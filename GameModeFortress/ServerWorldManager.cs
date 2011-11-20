﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ManicDigger;
using Vector3iG = ManicDigger.Vector3i;
using Vector3iC = ManicDigger.Vector3i;
using PointG = System.Drawing.Point;
using GameModeFortress;
using System.Diagnostics;

namespace ManicDiggerServer
{
    public partial class Server
    {
        //The main function for loading, unloadnig and sending chunks to players.
        private void NotifyMap()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            foreach (var k in clients)
            {
                if (k.Value.state == ClientStateOnServer.Connecting)
                {
                    continue;
                }
                var chunksAround = new List<Vector3i>(PlayerAreaChunks(k.Key));
                Vector3i playerpos = PlayerBlockPosition(k.Value);
                //a) if player is loading, then first generate all (LoadingGenerating), and then send all (LoadingSending)
                //b) if player is playing, then load 1, send 1.
                if (k.Value.state == ClientStateOnServer.LoadingGenerating)
                {
                    //load
                    for (int i = 0; i < chunksAround.Count; i++)
                    {
                        Vector3i v = chunksAround[i];
                        LoadChunk(v);
                        if (k.Value.state == ClientStateOnServer.LoadingGenerating)
                        {
                            var a = PlayerArea(k.Key);
                            if (i % 10 == 0)
                            {
                                SendLevelProgress(k.Key, (int)(((float)i / chunksAround.Count) * 100), "Generating world...");
                            }
                        }
                        if (s.ElapsedMilliseconds > 10)
                        {
                            return;
                        }
                    }
                    k.Value.state = ClientStateOnServer.LoadingSending;
                }
                else if (k.Value.state == ClientStateOnServer.LoadingSending)
                {
                    //send
                    for (int i = 0; i < chunksAround.Count; i++)
                    {
                        Vector3i v = chunksAround[i];

                        if (!k.Value.chunksseen.ContainsKey(v))
                        {
                            SendChunk(k.Key, v);
                            SendLevelProgress(k.Key, (int)(((float)k.Value.maploadingsentchunks++ / chunksAround.Count) * 100), "Downloading map...");
                            if (s.ElapsedMilliseconds > 10)
                            {
                                return;
                            }
                        }
                    }
                    //Finished map loading for a connecting player.
                    bool sent_all_in_range = (k.Value.maploadingsentchunks == chunksAround.Count);
                    if (sent_all_in_range)
                    {
                        SendLevelFinalize(k.Key);
                        clients[k.Key].state = ClientStateOnServer.Playing;
                    }
                }
                else //b)
                {
                    chunksAround.AddRange(ChunksAroundPlayer(playerpos));
                    //chunksAround.Sort((a, b) => DistanceSquared(a, playerpos).CompareTo(DistanceSquared(b, playerpos)));
                    for (int i = 0; i < chunksAround.Count; i++)
                    {
                        Vector3i v = chunksAround[i];
                        //load
                        LoadChunk(v);
                        //send
                        if (!k.Value.chunksseen.ContainsKey(v))
                        {
                            SendChunk(k.Key, v);
                        }
                        if (s.ElapsedMilliseconds > 10)
                        {
                            return;
                        }
                    }
                }
            }
        }

        void SendChunk(int clientid, Vector3i v)
        {
            Client c = clients[clientid];
            byte[] chunk = d_Map.GetChunk(v.x, v.y, v.z);
            c.chunksseen[v] = (int)simulationcurrentframe;
            //sent++;
            byte[] compressedchunk;
            if (MapUtil.IsSolidChunk(chunk) && chunk[0] == 0)
            {
                //don't send empty chunk.
                compressedchunk = null;
            }
            else
            {
                compressedchunk = CompressChunkNetwork(chunk);
                if (!c.heightmapchunksseen.ContainsKey(new Vector2i(v.x, v.y)))
                {
                    byte[] heightmapchunk = d_Map.GetHeightmapChunk(v.x, v.y);
                    byte[] compressedHeightmapChunk = d_NetworkCompression.Compress(heightmapchunk);
                    PacketServerHeightmapChunk p1 = new PacketServerHeightmapChunk()
                    {
                        X = v.x,
                        Y = v.y,
                        SizeX = chunksize,
                        SizeY = chunksize,
                        CompressedHeightmap = compressedHeightmapChunk,
                    };
                    SendPacket(clientid, Serialize(new PacketServer() { PacketId = ServerPacketId.HeightmapChunk, HeightmapChunk = p1 }));
                    c.heightmapchunksseen.Add(new Vector2i(v.x, v.y), (int)simulationcurrentframe);
                }
            }
            PacketServerChunk p = new PacketServerChunk()
            {
                X = v.x,
                Y = v.y,
                Z = v.z,
                SizeX = chunksize,
                SizeY = chunksize,
                SizeZ = chunksize,
                CompressedChunk = compressedchunk,
            };
            SendPacket(clientid, Serialize(new PacketServer() { PacketId = ServerPacketId.Chunk, Chunk = p }));
        }

        int playerareasize = 256;
        int centerareasize = 128;

        PointG PlayerArea(int playerId)
        {
            Point p = PlayerCenterArea(playerId);
            int x = p.X + centerareasize / 2;
            int y = p.Y + centerareasize / 2;
            x -= playerareasize / 2;
            y -= playerareasize / 2;
            return new Point(x, y);
        }

        PointG PlayerCenterArea(int playerId)
        {
            var pos = PlayerBlockPosition(clients[playerId]);
            int px = pos.x;
            int py = pos.y;
            int gridposx = (px / centerareasize) * centerareasize;
            int gridposy = (py / centerareasize) * centerareasize;
            return new Point(gridposx, gridposy);
        }

        IEnumerable<Vector3iG> PlayerAreaChunks(int playerId)
        {
            PointG p = PlayerArea(playerId);
            for (int x = 0; x < playerareasize / chunksize; x++)
            {
                for (int y = 0; y < playerareasize / chunksize; y++)
                {
                    for (int z = 0; z < d_Map.MapSizeZ / chunksize; z++)
                    {
                        yield return new Vector3i(p.X + x * chunksize, p.Y + y * chunksize, z * chunksize);
                    }
                }
            }
        }
    }
}

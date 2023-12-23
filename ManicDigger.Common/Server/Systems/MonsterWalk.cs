using System;


namespace ManicDigger.Server
{
    /// <summary>
    /// TODO: Implement monster movement
    /// </summary>
    class ServerSystemMonsterWalk : ServerSystem
    {
        int[] monsters;
        EntityAI[] ais;
        Controls[] controls;
        int player;
        int spawned;
        public ServerSystemMonsterWalk()
        {
            monsters = new int[255];
            ais = new EntityAI[255];
            for (int i = 0; i < spawned; i++)
            {
                ais[i] = new EntityAI();
            }
                controls = new Controls[255];
            spawned = 0;
            //for physics
            tmpPlayerPosition = new float[3];
            tmpBlockingBlockType = new IntRef();
            constWallDistance = 0.3f;
            player = -1;
        }
        float constWallDistance;
        Server s;
        public override void Update(Server server, float dt)
        {
            s = server;
            ModManager m = server.modManager;
            for (int i = 0; i < spawned; i++) {
                int p = monsters[i];

                EntityPosition_ pos = new EntityPosition_();

                pos.x = m.GetPlayerPositionX(p);
                pos.y = m.GetPlayerPositionY(p);
                pos.z = m.GetPlayerPositionZ(p);

                float toDest = server.Dist(pos.x, pos.y, pos.z,
                     ais[i].destination.x + one / 2, ais[i].destination.y - one / 2, ais[i].destination.z + one / 2);

                ais[i].movespeednow =  MoveSpeedNow(ais[i].stateplayerposition);

                ais[i].controls.movedx = MathCi.ClampFloat(ais[i].controls.movedx, -1, 1);
                ais[i].controls.movedy = MathCi.ClampFloat(ais[i].controls.movedy, -1, 1);
                ais[i].jumpstartacceleration = 13.333f * ais[i].constGravity; // default
                ais[i].jumpstartaccelerationhalf = 9 * ais[i].constGravity;
                ais[i].acceleration.SetDefault();

                if (toDest >= 1)
                {
                    ais[i].controls.movedy += 1;
                    if (ais[i].reachedwall)
                    {
                        ais[i].controls.wantsjump = true;
                    }
                    ////player orientation
                    float qX = ais[i].destination.x - ais[i].stateplayerposition.x;
                    float qY = ais[i].destination.y - ais[i].stateplayerposition.y;
                    float qZ = ais[i].destination.z - ais[i].stateplayerposition.z;
                    //float qY = game.playerdestination.Y - game.player.position.y;
                    //float qZ = game.playerdestination.Z - game.player.position.z;
                    float angle = VectorAngleGet(qX, qY, qZ);
                     //game.player.position.roty = Game.GetPi() / 2 + angle;
                    //game.player.position.rotx = Game.GetPi();
                    ais[i].stateplayerposition.rotx = MathCi.GetPi();
                    ais[i].stateplayerposition.roty = MathCi.GetPi()/2 +angle;
                }
                BoolRef sound=new BoolRef();
                Update(ais[i], dt, sound, Vector3Ref.Create(0, 0, 0), s.clients[p].entity.drawModel.modelHeight, i);
       
                m.SetPlayerPosition(p, ais[i].stateplayerposition.x, ais[i].stateplayerposition.y, ais[i].stateplayerposition.z);
                m.SetPlayerOrientation(p,(int)MathCi.RadToAngle256(ais[i].stateplayerposition.roty), (int)MathCi.RadToAngle256(ais[i].stateplayerposition.rotx), 0);

            }
        }
        internal float VectorAngleGet(float qX, float qY, float qZ)
        {
            return (float)(Math.Acos (qX / Length(qX, qY, qZ)) * MathCi.Sign(qZ));
        }
        internal float Length(float x, float y, float z)
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
        public override bool OnCommand(Server server, int sourceClientId, string command, string argument)
        {
            if (command == "monster")
            {
                server.SendMessage(sourceClientId, "spawned ");
                var pos = server.clients[sourceClientId].entity.position;
                SpawnMonster(server,pos.x, pos.y, pos.z);
                player = sourceClientId;
                  return true;

            }
            if (command == "rot")
            {

                float x = server.modManager.GetPlayerPositionX(sourceClientId);
                float y = server.modManager.GetPlayerPositionY(sourceClientId);
                float z = server.modManager.GetPlayerPositionZ(sourceClientId);
  

               int heading = server.modManager.GetPlayerHeading(sourceClientId);
                int pitch = server.modManager.GetPlayerPitch(sourceClientId);
                for (int i = 0; i < spawned; i++)
                {
                    int p = monsters[i];
                    ais[i].stateplayerposition.x = x;
                    ais[i].stateplayerposition.y = y;
                    ais[i].stateplayerposition.z = z;
                    ais[i].stateplayerposition.rotx = MathCi.Angle256ToRad(pitch);
                    ais[i].stateplayerposition.roty = MathCi.Angle256ToRad(heading);

                    server.modManager.SetPlayerPosition(p, x, y, z);
                    server.modManager.SetPlayerOrientation(p, heading, pitch, 0);

                }
                player = sourceClientId;
                return true;

            }
            if (command == "dest")
            {
                server.SendMessage(sourceClientId, "dest");
                var pos = server.clients[sourceClientId].entity.position;
                for (int i = 0; i < spawned; i++)
                {
                    ais[i].destination = new EntityPosition_();
                    ais[i].destination.x = pos.x;
                    ais[i].destination.y = pos.z;
                    ais[i].destination.z=pos.y;
                }
                    return true;
            }
            return false;
        }

        public void SpawnMonster(Server server, float x, float y, float z) {
             monsters[spawned] = server.modManager.AddBot("Zombie");
            ais[spawned] = new EntityAI();
            ais[spawned].stateplayerposition.x = x;
            ais[spawned].stateplayerposition.y = z;
            ais[spawned].stateplayerposition.z = y;

            server.modManager.SetPlayerPosition(monsters[spawned], x, y, z);

            spawned++;
        }

        public void Update( EntityAI ai, float dt, BoolRef soundnow, Vector3Ref push, float modelheight,int i)
        {
             // if (game.stopPlayerMove)

            //ai.movedz = 0;
            //      game.stopPlayerMove = false;


            // No air control
            if (!ai.isEntityonground)
            {
              ai.acceleration.acceleration1 = 0.99f;
              ai.acceleration.acceleration2 = 0.2f;
              ai.acceleration.acceleration3 = 70;
            }

            // Trampoline
            {
                int blockunderplayer = BlockUnderPlayer(ai.stateplayerposition);
                if (blockunderplayer != -1 && blockunderplayer == s.d_Data.BlockIdTrampoline()
                    && (!ai.isEntityonground) && !ai.controls.shiftkeydown)
                {
                    ai.controls.wantsjump = true;
                    ai.jumpstartacceleration = 20.666f * ai.constGravity;
                }
            }

            // Slippery walk on ice and when swimming
            {
                int blockunderplayer = BlockUnderPlayer(ai.stateplayerposition);
                if ((blockunderplayer != -1 && s.d_Data.IsSlipperyWalk(blockunderplayer)) || SwimmingBody(ai.stateplayerposition))
                {
                  ai.acceleration.acceleration1 = 0.99f;
                  ai.acceleration.acceleration2 = 0.2f;
                  ai.acceleration.acceleration3 = 70;
                }
            }

            soundnow.value = false;
            Vector3Ref diff1ref = new Vector3Ref();
            VectorTool.ToVectorInFixedSystem
                (ai.controls.movedx *ai.movespeednow * dt,
                 0,
                 ai.controls.movedy *ai.movespeednow * dt, ai.stateplayerposition.rotx, ai.stateplayerposition.roty, diff1ref);
            Vector3Ref diff1 = new Vector3Ref();
            diff1.X = diff1ref.X;
            diff1.Y = diff1ref.Y;
            diff1.Z = diff1ref.Z;
            if (VectorTool.Vec3Length(push.X, push.Y, push.Z) > 0.01f)
            {
                push.Normalize();
                push.X *= 5;
                push.Y *= 5;
                push.Z *= 5;
            }
            diff1.X += push.X * dt;
            diff1.Y += push.Y * dt;
            diff1.Z += push.Z * dt;

            bool loaded = false;
            int cx = FloatToInt(ai.stateplayerposition.x / Game.chunksize);
            int cy = FloatToInt(ai.stateplayerposition.z / Game.chunksize);
            int cz = FloatToInt(ai.stateplayerposition.y / Game.chunksize);
            if (MapUtil.IsValidPos(s.d_Map,cx,cy,cz ))
            {
                     

                if (s.d_Map.GetChunkValidSafe(cx, cy, cz) != null)
                {
                    loaded = true;
                }
                Console.WriteLine("NULL");

            }
            else
            {
                loaded = true;
            }
            if ((!(ai.controls.freemove)) && loaded)
            {
                if (!SwimmingBody(ai.stateplayerposition))
                {
                    ai.movedz += -ai.constGravity;//gravity
                }
                else
                {
                    ai.movedz += -ai.constGravity * ai.constWaterGravityMultiplier; //more gravity because it's slippery.
                }
            }

            if (ai.constEnableAcceleration)
            {
                ai.curspeed.X *= ai.acceleration.acceleration1;
                ai.curspeed.Y *= ai.acceleration.acceleration1;
                ai.curspeed.Z *= ai.acceleration.acceleration1;
                ai.curspeed.X = MakeCloserToZero(ai.curspeed.X, ai.acceleration.acceleration2 * dt);
                ai.curspeed.Y = MakeCloserToZero(ai.curspeed.Y, ai.acceleration.acceleration2 * dt);
                ai.curspeed.Z = MakeCloserToZero(ai.curspeed.Z, ai.acceleration.acceleration2 * dt);
                diff1.Y += ai.controls.moveup ? 2 * ai.movespeednow * dt : 0;
                diff1.Y -= ai.controls.movedown ? 2 * ai.movespeednow * dt : 0;
                ai.curspeed.X += diff1.X * ai.acceleration.acceleration3 * dt;
                ai.curspeed.Y += diff1.Y * ai.acceleration.acceleration3 * dt;
                ai.curspeed.Z += diff1.Z * ai.acceleration.acceleration3 * dt;
                if (ai.curspeed.Length() > ai.movespeednow)
                {
                    ai.curspeed.Normalize();
                    ai.curspeed.X *= ai.movespeednow;
                    ai.curspeed.Y *= ai.movespeednow;
                    ai.curspeed.Z *= ai.movespeednow;
                }
            }
            else
            {
                if (VectorTool.Vec3Length(diff1.X, diff1.Y, diff1.Z) > 0)
                {
                    diff1.Normalize();
                }
                ai.curspeed.X = diff1.X * ai.movespeednow;
                ai.curspeed.Y = diff1.Y * ai.movespeednow;
                ai.curspeed.Z = diff1.Z * ai.movespeednow;
            }
            Vector3Ref newposition = Vector3Ref.Create(0, 0, 0);
            if (!(ai.controls.freemove))
            {
                newposition.X = ai.stateplayerposition.x + ai.curspeed.X;
                newposition.Y =  ai.stateplayerposition.y + ai.curspeed.Y;
                newposition.Z =  ai.stateplayerposition.z + ai.curspeed.Z;
                if (!SwimmingBody( ai.stateplayerposition))
                {
                    newposition.Y =  ai.stateplayerposition.y;
                }
                // Fast move when looking at the ground
                float diffx = newposition.X -  ai.stateplayerposition.x;
                float diffy = newposition.Y -  ai.stateplayerposition.y;
                float diffz = newposition.Z -  ai.stateplayerposition.z;
                float difflength = VectorTool.Vec3Length(diffx, diffy, diffz);
                if (difflength > 0)
                {
                    diffx /= difflength;
                    diffy /= difflength;
                    diffz /= difflength;
                    diffx *= ai.curspeed.Length();
                    diffy *= ai.curspeed.Length();
                    diffz *= ai.curspeed.Length();
                }
                newposition.X =  ai.stateplayerposition.x + diffx * dt;
                newposition.Y =  ai.stateplayerposition.y + diffy * dt;
                newposition.Z =  ai.stateplayerposition.z + diffz * dt;
            }
            else
            {
                newposition.X =  ai.stateplayerposition.x + (ai.curspeed.X) * dt;
                newposition.Y =  ai.stateplayerposition.y + (ai.curspeed.Y) * dt;
                newposition.Z =  ai.stateplayerposition.z + (ai.curspeed.Z) * dt;
            }
            newposition.Y += ai.movedz * dt;
            Vector3Ref previousposition = Vector3Ref.Create( ai.stateplayerposition.x,  ai.stateplayerposition.y,  ai.stateplayerposition.z);
            if (!ai.controls.noclip)
            {
                float[] v = WallSlide(
                    Vec3.FromValues( ai.stateplayerposition.x,  ai.stateplayerposition.y,  ai.stateplayerposition.z),
                    Vec3.FromValues(newposition.X, newposition.Y, newposition.Z),
                    modelheight,ai);
                 ai.stateplayerposition.x = v[0];
                 ai.stateplayerposition.y = v[1];
                 ai.stateplayerposition.z = v[2];
            }
            else
            {
                 ai.stateplayerposition.x = newposition.X;
                 ai.stateplayerposition.y = newposition.Y;
                 ai.stateplayerposition.z = newposition.Z;
            }
            if (!(ai.controls.freemove))
            {
                if ((ai.isEntityonground) ||  SwimmingBody( ai.stateplayerposition))
                {
                    ai.jumpacceleration = 0;
                    ai.movedz = 0;
                }
                if ((ai.controls.wantsjump || ai.controls.wantsjumphalf) && (((ai.jumpacceleration == 0 && ai.isEntityonground) || SwimmingBody( ai.stateplayerposition)) && loaded)  )
                {
                    ai.jumpacceleration = ai.controls.wantsjumphalf ? ai.jumpstartaccelerationhalf : ai.jumpstartacceleration;
                    soundnow.value = true;
                }

                if (ai.jumpacceleration > 0)
                {
                    ai.isEntityonground = false;
                    ai.jumpacceleration = ai.jumpacceleration / 2;
                }

                //if (!this.reachedceiling)
                {
                    ai.movedz += ai.jumpacceleration * ai.constJump;
                }
            }
            else
            {
                ai.isEntityonground = true;
            }
            ais[i] = ai;

        }


    public bool IsTileEmptyForPhysics(int x, int y, int z)
        {
            if (z >= s.d_Map.MapSizeZ)
            {
                Console.WriteLine(" z >= s.d_Map.MapSize+" + "z: "+ z+": size :"+ s.d_Map.MapSizeZ);
                Console.WriteLine(" x  :"+ x+" y : " + y +" z :" + z);

                return true;
            }
            bool enableFreemove = false;
            if (x < 0 || y < 0 || z < 0)// || z >= mapsizez)
            {
                return enableFreemove;
            }
            if (x >= s.d_Map.MapSizeX || y >= s.d_Map.MapSizeY)// || z >= mapsizez)
            {
                return enableFreemove;
            }
            bool isvalid = MapUtil.IsValidPos(s.d_Map, x, y, z);
             if (!isvalid)
                return true;
            int block = s.d_Map.GetBlock(x, y, z);
            if (block == 0)
            {
                return true;
            }
            Packet_BlockType blocktype = s.d_Data.GetBlockType(block);
            return blocktype.WalkableType == Packet_WalkableTypeEnum.Fluid
                || s.d_Data.IsEmptyForPhysics(blocktype)
                || s.d_Data.IsRail(blocktype);
        }

        float[] tmpPlayerPosition;      //Temporarily stores the player's position. Used in WallSlide()
        IntRef tmpBlockingBlockType;
        public float[] WallSlide(float[] oldposition, float[] newposition, float modelheight,EntityAI ai)
        {


            bool high = false;
            if (modelheight >= 2) { high = true; }  //Set high to true if player model is bigger than standard height
            oldposition[1] +=  constWallDistance;       //Add walldistance temporarily for ground collisions
            newposition[1] +=  constWallDistance;       //Add walldistance temporarily for ground collisions

              ai.reachedwall = false;
              ai.reachedwall_1blockhigh = false;
             ai.reachedHalfBlock = false;

            tmpPlayerPosition[0] = oldposition[0];
            tmpPlayerPosition[1] = oldposition[1];
            tmpPlayerPosition[2] = oldposition[2];

            tmpBlockingBlockType.value = 0;

            // X
            if (IsEmptySpaceForPlayer(high, newposition[0], tmpPlayerPosition[1], tmpPlayerPosition[2], tmpBlockingBlockType))
            {
                tmpPlayerPosition[0] = newposition[0];
            }
            else
            {
                // For autojump
               ai.reachedwall = true;
                if (IsEmptyPoint(newposition[0], tmpPlayerPosition[1] + 0.5f, tmpPlayerPosition[2], null))
                {
                     ai.reachedwall_1blockhigh = true;
                    if (s.d_Data.GetBlockType( tmpBlockingBlockType.value).DrawType == Packet_DrawTypeEnum.HalfHeight) { ai.reachedHalfBlock = true; }
                    if (StandingOnHalfBlock(newposition[0], tmpPlayerPosition[1], tmpPlayerPosition[2])) {  ai.reachedHalfBlock = true; }
                }
            }
            // Y
            if (IsEmptySpaceForPlayer(high, tmpPlayerPosition[0], newposition[1], tmpPlayerPosition[2], tmpBlockingBlockType))
            {
                tmpPlayerPosition[1] = newposition[1];
            }
            // Z
            if (IsEmptySpaceForPlayer(high, tmpPlayerPosition[0], tmpPlayerPosition[1], newposition[2], tmpBlockingBlockType))
            {
                tmpPlayerPosition[2] = newposition[2];
            }
            else
            {
                // For autojump
                 ai.reachedwall = true;
                if (IsEmptyPoint(tmpPlayerPosition[0], tmpPlayerPosition[1] + 0.5f, newposition[2], null))
                {
                     ai.reachedwall_1blockhigh = true;
                    if (s.d_Data.GetBlockType(tmpBlockingBlockType.value).DrawType == Packet_DrawTypeEnum.HalfHeight) { ai.reachedHalfBlock = true; }
                    if (StandingOnHalfBlock(tmpPlayerPosition[0], tmpPlayerPosition[1], newposition[2])) { ai.reachedHalfBlock = true; }
                }
            }

            ai.isEntityonground = (tmpPlayerPosition[1] == oldposition[1]) && (newposition[1] < oldposition[1]);

            tmpPlayerPosition[1] -= constWallDistance; //Remove the temporary walldistance again
            return tmpPlayerPosition;   //Return valid position
        }

        bool StandingOnHalfBlock(float x, float y, float z)
        {
            int under = s.d_Map.GetBlock(FloatToInt(x),
                                          FloatToInt(z),
                                           FloatToInt(y));
            return s.d_Data.GetBlockType(under).DrawType == Packet_DrawTypeEnum.HalfHeight;
        }

        bool IsEmptySpaceForPlayer(bool high, float x, float y, float z, IntRef blockingBlockType)
        {
            return IsEmptyPoint(x, y, z, blockingBlockType)
                && IsEmptyPoint(x, y + 1, z, blockingBlockType)
                && (!high || IsEmptyPoint(x, y + 2, z, blockingBlockType));
        }

        // Checks if there are no solid blocks in walldistance area around the point
        bool IsEmptyPoint(float x, float z, float y, IntRef blockingBlocktype)
        {
            // Test 3x3x3 blocks around the point
            for (int xx = 0; xx < 3; xx++)
            {
                for (int yy = 0; yy < 3; yy++)
                {
                    for (int zz = 0; zz < 3; zz++)
                    {
                        if (!IsTileEmptyForPhysics(FloatToInt(x + xx - 1), FloatToInt(z + zz - 1), FloatToInt(y + yy - 1)))
                        {
                            // Found a solid block

                            // Get bounding box of the block
                            float minX = FloatToInt(x + xx - 1);
                            float minY = FloatToInt(y + yy - 1);
                            float minZ = FloatToInt(z + zz - 1);
                            float maxX = minX + 1;
                            float maxY = minY + getblockheight(FloatToInt(x + xx - 1), FloatToInt(z + zz - 1), FloatToInt(y + yy - 1));
                            float maxZ = minZ + 1;

                            // Check if the block is too close
                            if (BoxPointDistance(minX, minY, minZ, maxX, maxY, maxZ, x, y, z) < constWallDistance)
                            {
                                if (blockingBlocktype != null)
                                {
                                    blockingBlocktype.value = s.d_Map.GetBlock(FloatToInt(x + xx - 1), FloatToInt(z + zz - 1), FloatToInt(y + yy - 1));
                                }
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        // Using chebyshev distance
        public static float BoxPointDistance(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float pX, float pY, float pZ)
        {
            float dx = Max3(minX - pX, 0, pX - maxX);
            float dy = Max3(minY - pY, 0, pY - maxY);
            float dz = Max3(minZ - pZ, 0, pZ - maxZ);
            return Max3(dx, dy, dz);
        }

        public static float MakeCloserToZero(float a, float b)
        {
            if (a > 0)
            {
                float c = a - b;
                if (c < 0)
                {
                    c = 0;
                }
                return c;
            }
            else
            {
                float c = a + b;
                if (c > 0)
                {
                    c = 0;
                }
                return c;
            }
        }

        static float Max3(float a, float b, float c)
        {
            return MathCi.MaxFloat(MathCi.MaxFloat(a, b), c);
        }
        int FloatToInt(float value)
        {
            return  (int)(value);
        }
        public float getblockheight(int x, int y, int z)
        {
            float RailHeight = one * 3 / 10;
            if (!MapUtil.IsValidPos(s.d_Map, x, y, z))
            {
                return 1;
            }
            return s.d_Data.getblockheight(s.d_Map.GetBlock(x, y, z));
        }
        internal int BlockUnderPlayer(EntityPosition_ stateplayerposition)
        {

            if (!MapUtil.IsValidPos(s.d_Map, FloatToInt(stateplayerposition.x),
                FloatToInt(stateplayerposition.z),
                FloatToInt(stateplayerposition.y) - 1))
            {
                return -1;
            }
            int blockunderplayer = s.d_Map.GetBlock(FloatToInt(stateplayerposition.x),
               FloatToInt(stateplayerposition.z),
                FloatToInt(stateplayerposition.y) - 1);
            return blockunderplayer;
        }
        internal bool SwimmingBody(EntityPosition_ stateplayerposition)
        {
            if (!MapUtil.IsValidPos(s.d_Map, FloatToInt(stateplayerposition.x),
           FloatToInt(stateplayerposition.z),
           FloatToInt(stateplayerposition.y) - 1))
            {
                return true;
            }
            int block = s.d_Map.GetBlock( FloatToInt(stateplayerposition.x), FloatToInt(stateplayerposition.z), FloatToInt(stateplayerposition.y + 1));
            if (block == -1) { return true; }
            return s.d_Data.WalkableType1(block) == Packet_WalkableTypeEnum.Fluid;
        }


        internal float MoveSpeedNow(EntityPosition_ stateplayerposition)
        {
            float movespeednow = 5;

            {
                //walk faster on cobblestone
                int blockunderplayer = BlockUnderPlayer(stateplayerposition);
                if (blockunderplayer != -1)
                {
                    float floorSpeed = s.d_Data.WalkSpeed(blockunderplayer);
                    if (floorSpeed != 0)
                    {
                        movespeednow *= floorSpeed;
                    }
                }
            }

   
            return movespeednow;
        }


    }

}

public class EntityAI : EntityEngine
{
    public Controls controls;
    public EntityPosition_ destination;
    public EntityPosition_ stateplayerposition;

    public bool reachedwall = false;
    public bool reachedwall_1blockhigh = false;
    public bool reachedHalfBlock = false;

    public EntityAI()
    {
        controls=new Controls();
        destination= new EntityPosition_();
        stateplayerposition = new EntityPosition_();
        constGravity = 0.3f;
        constWaterGravityMultiplier = 3;
        constEnableAcceleration = true;
        constJump = 2.1f;
        isEntityonground = false;

        movedz = 0;
        jumpacceleration = 0;
        acceleration = new Acceleration();
        jumpstartacceleration = 0;
        jumpstartaccelerationhalf = 0;
        movespeednow = 0;
        curspeed = new Vector3Ref();

    }

   
}

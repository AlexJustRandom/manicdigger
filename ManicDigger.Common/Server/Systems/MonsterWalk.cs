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
        int spawned;
        public ServerSystemMonsterWalk()
        {
            monsters = new int[255];
            ais = new EntityAI[255];
            controls = new Controls[255];
            spawned = 0;
            //for physics
            tmpPlayerPosition = new float[3];
            tmpBlockingBlockType = new IntRef();
            constWallDistance = 0.3f;
            isEntityonground = false;
        }
        bool isEntityonground;
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


                if (toDest >= 1)
                {
                    ais[i].controls.movedy += 1;
                    if (ais[i].reachedwall)
                    {
                        ais[i].controls.wantsjump = true;
                    }
                    ////player orientation
                    float qX = ais[i].destination.X - ais[i].stateplayerposition.x;
                    //float qY = game.playerdestination.Y - game.player.position.y;
                    //float qZ = game.playerdestination.Z - game.player.position.z;
                    //float angle = game.VectorAngleGet(qX, qY, qZ);
                    //game.player.position.roty = Game.GetPi() / 2 + angle;
                    //game.player.position.rotx = Game.GetPi();
                }

            }
        }
 

        public override bool OnCommand(Server server, int sourceClientId, string command, string argument)
        {
            if (command == "monster")
            {
                server.SendMessage(sourceClientId, "spawned ");
                var pos = server.clients[sourceClientId].entity.position;
                SpawnMonster(server,pos.x, pos.y, pos.z);
                 return true;
            }
            return false;
        }

        public void SpawnMonster(Server server, float x, float y, float z) {
             monsters[spawned] = server.modManager.AddBot("Zombie");
            server.modManager.SetPlayerPosition(monsters[spawned], x, y, z);

            spawned++;
        }

        public void Update( EntityAI ai, float dt, BoolRef soundnow, Vector3Ref push, float modelheight)
        {
            EntityEngine engine = new EntityEngine();
            // if (game.stopPlayerMove)

            engine.movedz = 0;
            //      game.stopPlayerMove = false;


            // No air control
            if (!isEntityonground)
            {
              engine.acceleration.acceleration1 = 0.99f;
              engine.acceleration.acceleration2 = 0.2f;
              engine.acceleration.acceleration3 = 70;
            }

            // Trampoline
            {
                int blockunderplayer = BlockUnderPlayer(ai.stateplayerposition);
                if (blockunderplayer != -1 && blockunderplayer == s.d_Data.BlockIdTrampoline()
                    && (!isEntityonground) && !ai.controls.shiftkeydown)
                {
                    ai.controls.wantsjump = true;
                    engine.jumpstartacceleration = 20.666f * engine.constGravity;
                }
            }

            // Slippery walk on ice and when swimming
            {
                int blockunderplayer = BlockUnderPlayer(ai.stateplayerposition);
                if ((blockunderplayer != -1 && s.d_Data.IsSlipperyWalk(blockunderplayer)) || SwimmingBody(ai.stateplayerposition))
                {
                  engine.acceleration.acceleration1 = 0.99f;
                  engine.acceleration.acceleration2 = 0.2f;
                  engine.acceleration.acceleration3 = 70;
                }
            }

            soundnow.value = false;
            Vector3Ref diff1ref = new Vector3Ref();
            VectorTool.ToVectorInFixedSystem
                (ai.controls.movedx *engine.movespeednow * dt,
                 0,
                 ai.controls.movedy *engine.movespeednow * dt, ai.stateplayerposition.rotx, ai.stateplayerposition.roty, diff1ref);
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
            int cy = FloatToInt(ai.stateplayerposition.y / Game.chunksize);
            int cz = FloatToInt(ai.stateplayerposition.z / Game.chunksize);
            if (MapUtil.IsValidPos(s.d_Map,cx,cy,cz ))
            {
               
                if (s.d_Map.GetChunkValid(cx, cy, cz) != null)
                {
                    loaded = true;
                }
            }
            else
            {
                loaded = true;
            }
            if ((!(ai.controls.freemove)) && loaded)
            {
                if (!SwimmingBody(ai.stateplayerposition))
                {
                    engine.movedz += -engine.constGravity;//gravity
                }
                else
                {
                    engine.movedz += -engine.constGravity * engine.constWaterGravityMultiplier; //more gravity because it's slippery.
                }
            }

            if (engine.constEnableAcceleration)
            {
                engine.curspeed.X *= engine.acceleration.acceleration1;
                engine.curspeed.Y *= engine.acceleration.acceleration1;
                engine.curspeed.Z *= engine.acceleration.acceleration1;
                engine.curspeed.X = MakeCloserToZero(engine.curspeed.X, engine.acceleration.acceleration2 * dt);
                engine.curspeed.Y = MakeCloserToZero(engine.curspeed.Y, engine.acceleration.acceleration2 * dt);
                engine.curspeed.Z = MakeCloserToZero(engine.curspeed.Z, engine.acceleration.acceleration2 * dt);
                diff1.Y += ai.controls.moveup ? 2 * engine.movespeednow * dt : 0;
                diff1.Y -= ai.controls.movedown ? 2 * engine.movespeednow * dt : 0;
                engine.curspeed.X += diff1.X * engine.acceleration.acceleration3 * dt;
                engine.curspeed.Y += diff1.Y * engine.acceleration.acceleration3 * dt;
                engine.curspeed.Z += diff1.Z * engine.acceleration.acceleration3 * dt;
                if (engine.curspeed.Length() > engine.movespeednow)
                {
                    engine.curspeed.Normalize();
                    engine.curspeed.X *= engine.movespeednow;
                    engine.curspeed.Y *= engine.movespeednow;
                    engine.curspeed.Z *= engine.movespeednow;
                }
            }
            else
            {
                if (VectorTool.Vec3Length(diff1.X, diff1.Y, diff1.Z) > 0)
                {
                    diff1.Normalize();
                }
                engine.curspeed.X = diff1.X * engine.movespeednow;
                engine.curspeed.Y = diff1.Y * engine.movespeednow;
                engine.curspeed.Z = diff1.Z * engine.movespeednow;
            }
            Vector3Ref newposition = Vector3Ref.Create(0, 0, 0);
            if (!(ai.controls.freemove))
            {
                newposition.X = ai.stateplayerposition.x + engine.curspeed.X;
                newposition.Y =  ai.stateplayerposition.y + engine.curspeed.Y;
                newposition.Z =  ai.stateplayerposition.z + engine.curspeed.Z;
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
                    diffx *= engine.curspeed.Length();
                    diffy *= engine.curspeed.Length();
                    diffz *= engine.curspeed.Length();
                }
                newposition.X =  ai.stateplayerposition.x + diffx * dt;
                newposition.Y =  ai.stateplayerposition.y + diffy * dt;
                newposition.Z =  ai.stateplayerposition.z + diffz * dt;
            }
            else
            {
                newposition.X =  ai.stateplayerposition.x + (engine.curspeed.X) * dt;
                newposition.Y =  ai.stateplayerposition.y + (engine.curspeed.Y) * dt;
                newposition.Z =  ai.stateplayerposition.z + (engine.curspeed.Z) * dt;
            }
            newposition.Y += engine.movedz * dt;
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
                if ((isEntityonground) ||  SwimmingBody( ai.stateplayerposition))
                {
                    engine.jumpacceleration = 0;
                    engine.movedz = 0;
                }
                if ((ai.controls.wantsjump || ai.controls.wantsjumphalf) && (((engine.jumpacceleration == 0 && isEntityonground) || SwimmingBody( ai.stateplayerposition)) && loaded)  )
                {
                    engine.jumpacceleration = ai.controls.wantsjumphalf ? engine.jumpstartaccelerationhalf : engine.jumpstartacceleration;
                    soundnow.value = true;
                }

                if (engine.jumpacceleration > 0)
                {
                    isEntityonground = false;
                    engine.jumpacceleration = engine.jumpacceleration / 2;
                }

                //if (!this.reachedceiling)
                {
                    engine.movedz += engine.jumpacceleration * engine.constJump;
                }
            }
            else
            {
                isEntityonground = true;
            }


        }


    public bool IsTileEmptyForPhysics(int x, int y, int z)
        {
            if (z >= s.d_Map.MapSizeZ)
            {
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

            isEntityonground = (tmpPlayerPosition[1] == oldposition[1]) && (newposition[1] < oldposition[1]);

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
        bool IsEmptyPoint(float x, float y, float z, IntRef blockingBlocktype)
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
            int block = s.d_Map.GetBlock( FloatToInt(stateplayerposition.x), FloatToInt(stateplayerposition.z), FloatToInt(stateplayerposition.y + 1));
            if (block == -1) { return true; }
            return s.d_Data.WalkableType1(block) == Packet_WalkableTypeEnum.Fluid;
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

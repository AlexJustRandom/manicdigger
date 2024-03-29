using System;
using System.Collections.Generic;
//Imortant note :: Couple of mod funtions are caled FROM task sheduler

    public class ClientModloader
    {
        private ClientModManager modManager;

        public int GetClientModsCount{ get { return GetClientMods.Count; } }
        public List<ClientMod> GetClientMods { get; } = new List<ClientMod>();
        
        public void SetGame(Game game_)
        {
            modManager = new ClientModManager1{game = game_};
        }


    public void AddMod(ClientMod mod)
        {
        GetClientMods.Add(mod);
            mod.Start(modManager);
        }

        public void Dispose(Game game) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
            GetClientMods[i].Dispose(game);
            }
        }

        public void OnNewFrame(Game game, NewFrameEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
                GetClientMods[i].OnNewFrame(game, args);
            }
        }
        
        public void OnNewFrameFixed(Game game, NewFrameEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
                GetClientMods[i].OnNewFrameFixed(game, args);
            }
        }
        
        public void OnBeforeNewFrameDraw2d(Game game, float deltaTime) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
                GetClientMods[i].OnBeforeNewFrameDraw2d(game, deltaTime);
            }
        }
            
        public void OnNewFrameDraw2d(Game game, float deltaTime) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnNewFrameDraw2d(game, deltaTime);
            }
        }
        
        public void OnBeforeNewFrameDraw3d(Game game, float deltaTime) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnBeforeNewFrameDraw3d(game, deltaTime);
            }
        }
        
        public void OnNewFrameDraw3d(Game game, float deltaTime) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnNewFrameDraw3d(game, deltaTime);
            }
        }

        public bool OnKeyDown(Game game, KeyEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
                GetClientMods[i].OnKeyDown(game, args);
                if (args.GetHandled())
                {
                    return true;
                }
            }
        return false;
    }
        public bool OnKeyPress(Game game, KeyPressEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
                GetClientMods[i].OnKeyPress(game, args);
                if (args.GetHandled())
            {
                return true;
            }
        }
        return false;
    }
    public bool OnKeyUp(Game game, KeyEventArgs args)
        {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnKeyUp(game, args); if (args.GetHandled())
            {
                return true;
            }
        }
        return false;
    }
    public void OnMouseUp(Game game, MouseEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnMouseUp(game, args);
            }
        }
        public void OnMouseDown(Game game, MouseEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnMouseDown(game, args);
            }
        }
        public void OnMouseMove(Game game, MouseEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnMouseMove(game, args);

            }
        }
        public void OnMouseWheelChanged(Game game, MouseWheelEventArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnMouseWheelChanged(game, args);
            }
        }

        public void OnTouchStart(Game game, TouchEventArgs e) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnTouchStart(game, e);
                if (e.GetHandled())
                {
                    return;
                }
            }
        }
        public void OnTouchMove(Game game, TouchEventArgs e)
        {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnTouchMove(game, e);
                if (e.GetHandled())
                {
                    return;
                }
            }
        }
        public void OnTouchEnd(Game game, TouchEventArgs e)
        {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnTouchEnd(game, e);
                if (e.GetHandled())
                {
                    return;
                }
        }
        }
        public void OnUseEntity(Game game, OnUseEntityArgs e)
        {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnUseEntity(game, e);

            }
        }
        public void OnHitEntity(Game game, OnUseEntityArgs e)
        {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
            GetClientMods[i].OnHitEntity(game, e);
            }
        }

        public bool OnClientCommand(Game game, ClientCommandArgs args) {
            for (int i = 0; i < GetClientMods.Count; i++)
            {
                if (GetClientMods[i] == null) { continue; }
                if (GetClientMods[i].OnClientCommand(game, args)) return true;//Maybe todo
            }
            return false;
        }
        
    }

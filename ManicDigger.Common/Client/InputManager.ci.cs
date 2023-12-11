using System.Collections.Generic;
public class KeyBinding {
    public int keycode;
    public string name;
    public string description;
}
public static class Bindings{


    public static int MoveFoward;
    public static int MoveBack;
    public static int MoveLeft;
    public static int MoveRight;
    public static int Jump;
    public static int ShowMaterialSelector;
    public static int SetSpawnPosition;
    public static int Respawn;
    public static int ReloadWeapon;
    public static int ToggleFogDistance;
    public static int FreeMove;
    public static int ThirdPersonCamera;
    public static int TextEditor;
    public static int Fullscreen;
    public static int Screenshot;
    public static int PlayersList;
    public static int Chat;
    public static int TeamChat;
    public static int Craft;
    public static int BlockInfo;
    public static int Use;
    public static int ReverseMinecart;
}
public class InputManager
{
    public InputManager(Game g)
    {
        game = g;

        const int KeysMax = 256;
        keyboardState = new bool[KeysMax];
        keyboardStateRaw = new bool[KeysMax];
        for (int i = 0; i < KeysMax; i++)
        {
            keyboardState[i] = false;
            keyboardStateRaw[i] = false;
        }
        IsShiftPressed = false;


        Bindings.MoveFoward = AddInput("KeyMoveFoward", GlKeys.W, game.language.KeyMoveFoward());
        Bindings.MoveBack = AddInput("KeyMoveBack", GlKeys.S, game.language.KeyMoveBack());
        Bindings.MoveLeft = AddInput("KeyMoveLeft", GlKeys.A, game.language.KeyMoveLeft());
        Bindings.MoveRight = AddInput("KeyMoveRight", GlKeys.D, game.language.KeyMoveRight());
        Bindings.Jump = AddInput("KeyJump", GlKeys.Space, game.language.KeyJump());
        Bindings.ShowMaterialSelector = AddInput("KeyShowMaterialSelector", GlKeys.B, game.language.KeyShowMaterialSelector());
        Bindings.SetSpawnPosition = AddInput("KeySetSpawnPosition", GlKeys.P, game.language.KeySetSpawnPosition());
        Bindings.Respawn = AddInput("KeyRespawn", GlKeys.O, game.language.KeyRespawn());
        Bindings.ReloadWeapon = AddInput("KeyReloadWeapon", GlKeys.R, game.language.KeyReloadWeapon());
        Bindings.ToggleFogDistance = AddInput("KeyToggleFogDistance", GlKeys.F, game.language.KeyToggleFogDistance());
        Bindings.FreeMove = AddInput("KeyFreeMove", GlKeys.F3, game.language.KeyFreeMove());
        Bindings.ThirdPersonCamera = AddInput("KeyThirdPersonCamera", GlKeys.F5, game.language.KeyThirdPersonCamera());
        Bindings.TextEditor = AddInput("KeyTextEditor", GlKeys.F9, game.language.KeyTextEditor());
        Bindings.Fullscreen = AddInput("KeyFullscreen", GlKeys.F11, game.language.KeyFullscreen());
        Bindings.Screenshot = AddInput("KeyScreenshot", GlKeys.F12, game.language.KeyScreenshot());
        Bindings.PlayersList = AddInput("KeyPlayersList", GlKeys.Tab, game.language.KeyPlayersList());
        Bindings.Chat = AddInput("KeyChat", GlKeys.T, game.language.KeyChat());
        Bindings.TeamChat = AddInput("KeyTeamChat", GlKeys.Y, game.language.KeyTeamChat());
        Bindings.Craft = AddInput("KeyCraft", GlKeys.C, game.language.KeyCraft());
        Bindings.BlockInfo = AddInput("KeyBlockInfo", GlKeys.I, game.language.KeyBlockInfo());
        Bindings.Use = AddInput("KeyUse", GlKeys.E, game.language.KeyUse());
        Bindings.ReverseMinecart = AddInput("KeyReverseMinecart", GlKeys.Q, game.language.KeyReverseMinecart());
        AddInput("KeyMoveSpeed1", GlKeys.F1, game.platform.StringFormat(game.language.KeyMoveSpeed(), "1"));
        AddInput("KeyMoveSpeed10", GlKeys.F2, game.platform.StringFormat(game.language.KeyMoveSpeed(), "10"));
    }
    Game game;

    internal bool[] keyboardState;
    internal bool[] keyboardStateRaw;
    internal bool IsShiftPressed;

    // TODO change to hash map ? idk list ist stupid
    public List<KeyBinding> keymaps;
    
    public int GetInputID(string name) {
        for (int i = 0; i < keymaps.Count; i++) {
            if (name == keymaps[i].name) return i;
        }
        return -1;
    }

    public void ChangBinding(int bindingID, int keycode) {
        keymaps[bindingID].keycode = keycode;
    }

    public int AddInput(string name,int key,string desc) {
        KeyBinding binding = new KeyBinding();
        binding.name = name;
        binding.keycode = key;
        binding.description = desc;
        keymaps.Add(binding);
        return keymaps.Count - 1;
    }

    public bool IsInputDown(int bindingID)
    {
        if (bindingID == -1) return false; 
        return keyboardState[keymaps[bindingID].keycode];
    }




    internal void KeyUp(int eKey)
    {
        keyboardStateRaw[eKey] = false;
        for (int i = 0; i < game.clientmodsCount; i++)
        {
            KeyEventArgs args_ = new KeyEventArgs();
            args_.SetKeyCode(eKey);
            game.clientmods[i].OnKeyUp(game, args_);
            if (args_.GetHandled())
            {
                return;
            }
        }
        keyboardState[eKey] = false;
        if (eKey == game.GetKey(GlKeys.ShiftLeft) || eKey == game.GetKey(GlKeys.ShiftRight))
        {
            IsShiftPressed = false;
        }
    }

    internal void KeyDown(int eKey)
    {
        keyboardStateRaw[eKey] = true;
        if (game.guistate != GuiState.MapLoading)
        {
            // only handle keys once game has been loaded
            for (int i = 0; i < game.clientmodsCount; i++)
            {
                KeyEventArgs args_ = new KeyEventArgs();
                args_.SetKeyCode(eKey);
                game.clientmods[i].OnKeyDown(game, args_);
                if (args_.GetHandled())
                {
                    return;
                }
            }
        }

        keyboardState[eKey] = true;

        if (eKey == GetKey(GlKeys.ShiftLeft) || eKey == GetKey(GlKeys.ShiftRight))
        {
            IsShiftPressed = true;
        }

    }


    public bool IsCtrlDown
    {
        get {
            return (keyboardStateRaw[game.GetKey(GlKeys.ControlLeft)] || keyboardStateRaw[game.GetKey(GlKeys.ControlRight)]));
            }
    }


}
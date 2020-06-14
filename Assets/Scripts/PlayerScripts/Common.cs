using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Common
{
    public enum PlayerId { TouchPlayer, Player1, Player2, Player3, Player4 }
    public enum PlayerState { Jugando, EsperandoARevivir, Caido, Muerto }


    public enum PlayerInputButton { L1_Button, L2_Button, L3_Button, R1_Button, R2_Button, R3_Button, DPad_Up, DPad_Down, DPad_Left, DPad_Right, Square_Button, Cross_Button, Triangle_Button, Circle_Button, Share_Button, Options_Button, PS_Button, TouchPad_Button  }
    public enum PlayerInputAxis { DPad, LStick, RStick }

    public enum MainAttackType { Light, Medium, Fierce }
    public enum AttackReaction { Juggle, Pushback }

    public static int LightDamage = 10;
    public static int MediumDamage = 20;
    public static int HeavyDamage = 40;


    public enum EnemyState { Activo, Inactivo, Muerto, Juggle }
    public enum EnemyType { Cannon, Perseguidor, Exploder }
    public enum EnemyPriority { JugadorMasDebil, JugadorMasSano, JugadorMasLejano, JugadorMasCercano, Random }


    public static class Hash
    {

        public static string Player1_Input_Prefix = "P1";
        public static string Player2_Input_Prefix = "P2";
        public static string Player3_Input_Prefix = "P3";
        public static string Player4_Input_Prefix = "P4";

        public static string Cross_Button_Suffix = "_Cross";
        public static string Square_Button_Suffix = "_Square";
        public static string Circle_Button_Suffix = "_Circle";
        public static string Triangle_Button_Suffix = "_Triangle";

        public static string L1_Button_Suffix = "_L1_Button";
        public static string L2_Button_Suffix = "_L2_Button";
        public static string L3_Button_Suffix = "_L3_Button";

        public static string R1_Button_Suffix = "_L1_Button";
        public static string R2_Button_Suffix = "_L2_Button";
        public static string R3_Button_Suffix = "_L3_Button";

        public static string LStick_Vertical = "_LStick_UpDown";
        public static string LStick_Horizontal = "_LStick_LeftRight";

        public static string RStick_Vertical = "_RStick_UpDown";
        public static string RStick_Horizontal = "_RStick_LeftRight";

        public static string DPad_Vertical = "_DPad_UpDown";
        public static string DPad_Horizontal = "_DPad_LeftRight";
    }


}


namespace Schizo
{using System.ComponentModel;

    public class Config
    {
        public int ChanceSpawn { get; set; } = 100;
        public int MinPlayer { get; set; } = 0;
        public int MaxSpawn { get; set; } = 1;
        [Description("Per quanto tempo lo schizo non dovrebbe sentire voci dopo aver mangiato un painkiller")]
        public float NoVoci { get; set; } = 60f;

        public Main Sz = new Main();

    }
}
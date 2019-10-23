using System;

namespace OpenProjects.Model
{
    public class OverlayService : BaseVm
    {
        private static readonly OverlayService _Instance = new OverlayService();

        private OverlayService()
        {
        }

        public Item SelItem { get; set; }
        public Action<string, string> Show { get; set; }


        public string Text { get; set; } = "";


        public string Description { get; set; }

        public static OverlayService GetInstance()
        {
            return _Instance;
        }

        public void Close()
        {
            SelItem = null;
        }
    }
}
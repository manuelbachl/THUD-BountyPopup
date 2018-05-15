using System;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.MaBa.Bounties
{
    public class BountyPopupPlugin : BasePlugin, IInGameWorldPainter 
    {
        public TopLabelWithTitleDecorator BountyPopupDecorator { get; set; }

        public float RatioX { get; set; }
        public float RatioY { get; set; }
        public float RatioW { get; set; }
        public float RatioH { get; set; }

        public class Popup : IQueueItem
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public string Hint { get; set; }
            public DateTime QueuedOn { get; private set; }
            public TimeSpan LifeTime { get; private set; }
            public string Type { get; set; }
            public Popup(string text, string title, TimeSpan lifetime, string hint, string type = "Default")
            {
                this.Text = text;
                this.Title = title;
                this.LifeTime = lifetime;
                this.Hint = hint;
                this.QueuedOn = DateTime.Now;
                this.Type = type;
            }
        }

        public void Show(string text, string title, int duration, string hint = null)
        {
            Hud.Queue.AddItem(new Popup(text, title, new TimeSpan(0, 0, 0, 0, duration), hint));
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            BountyPopupDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                BorderBrush = Hud.Render.CreateBrush(255, 255, 23, 0, -1),
                BackgroundBrush = Hud.Render.CreateBrush(200, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 23, 0, true, false, false),
                TitleFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 23, 0, true, false, false),
            };

            RatioW = 0.36f;
            RatioH = 0.05f;
            RatioY = 0.15f;
            RatioX = 0.4f;
        }

        public void PaintWorld(WorldLayer layer)
        {
            var w = Hud.Window.Size.Height * RatioW;
            var h = Hud.Window.Size.Height * RatioH;
            var x = Hud.Window.Size.Width * RatioX;
            var y = Hud.Window.Size.Height * RatioY;

            foreach (Popup p in Hud.Queue.GetItems<Popup>().Take(1))
            {
                BountyPopupDecorator.Paint(x, y, w, h, p.Text, p.Title, p.Hint);
            }
        }

        public BountyPopupPlugin()
        {
            Enabled = true;
        }
    }
}
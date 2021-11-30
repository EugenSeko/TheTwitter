using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterTwitter.Views.Flyout
{
    public class FlyoutViewFlyoutMenuItem
    {
        public FlyoutViewFlyoutMenuItem()
        {
            TargetType = typeof(FlyoutViewFlyoutMenuItem);
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
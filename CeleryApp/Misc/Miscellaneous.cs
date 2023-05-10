using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Xml;

namespace CeleryApp.Misc
{
    public static class Miscellaneous
    {
        public static T GetTemplateChild<T>(this Control e, string name) where T : FrameworkElement
        {
            return e.Template.FindName(name, e) as T;
        }
    }
}

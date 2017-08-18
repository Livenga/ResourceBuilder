using System;
using System.Drawing;
using System.Windows.Forms;

using Live.Forms;


namespace Live
{
  public class ResourceBuilder
  {
    [STAThread]
    public static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.Run(new ResourceForm());
    }
  }
}

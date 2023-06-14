using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

// ESRI
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;

using MyArcObjectsApp1.Commands;


namespace MyArcObjectsApp1
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }

        
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            axMapControl1.Map.Name = "Map";
        }

        // 右键菜单
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button != 2) return;

            IMapControl3 mapControl = axMapControl1.Object as IMapControl3;
            IToolbarMenu toolMenu = new ToolbarMenuClass();
            // 移除图层
            toolMenu.AddItem(new RemoveLayerCommmand(), -1, 0, true, esriCommandStyles.esriCommandStyleTextOnly);
            toolMenu.SetHook(mapControl);

            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer lyr = null;
            object other = null;
            object index = null;

            this.axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref lyr, ref other, ref index);
            if (item == esriTOCControlItem.esriTOCControlItemMap)
                this.axTOCControl1.SelectItem(map, null);
            else if (item == esriTOCControlItem.esriTOCControlItemLayer)
                this.axTOCControl1.SelectItem(lyr, null);

            axMapControl1.CustomProperty = lyr;
            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                toolMenu.PopupMenu(e.x, e.y, this.axTOCControl1.hWnd);
            }
        }

        private void 打开Mxd文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择地图文档";
            ofd.Filter = "地图文档(*.mxd)|*mxd";
            ofd.InitialDirectory = @"C:\Users\39629\OneDrive\文档\Tencent Files\3962980\FileRecv\GIS2\data";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string mxdpath = ofd.FileName;
                axMapControl1.LoadMxFile(mxdpath);
            }
        }

        private void 导入图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand command = new ControlsAddDataCommandClass();
            command.OnCreate(this.axMapControl1.Object); // 设置Command的hook
            command.OnClick(); // 执行Command
        }

        private void 移除图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建 Form1 窗体的实例并使用 ShowDialog 方法打开它
            Form1 form = new Form1(axMapControl1);
            form.ShowDialog();
        }

        private void 要素符号化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMapControl3 mapControl = axMapControl1.Object as IMapControl3;
            InitializeLayers(mapControl);
            Form2 form2 = new Form2(_layers, axMapControl1);
            form2.Show();
        }
        private void InitializeLayers(IMapControl3 mapControl)
        {
            _layers = new List<ILayer>();
            IEnumLayer mapLayers = mapControl.Map.get_Layers(null, true);
            mapLayers.Reset();

            ILayer layer;
            while ((layer = mapLayers.Next()) != null)
            {
                _layers.Add(layer);
            }
        }
        private List<ILayer> _layers;


    }
}

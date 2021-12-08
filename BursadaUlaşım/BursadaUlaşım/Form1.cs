using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;

namespace BursadaUlaşım
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        vt_islemleri vt;
        GMap.NET.WindowsForms.GMapMarker marker;
        GMap.NET.WindowsForms.GMapMarker marker2;
        GMapOverlay overlay = new GMapOverlay("yapıcı");
        int durakid;
        int durakid2;
        List<PointLatLng> Pontos = new List<PointLatLng>();
        private void Form1_Load(object sender, EventArgs e)
        {
            vt = new vt_islemleri();
            gmap.ShowCenter = false;
            veriDoldur();
            haritagosterme();         
        }
        public void haritagosterme()
        {
            try
            {
                gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
                gmap.SetPositionByKeywords("Bursa, Turkey");

            }
            catch (Exception)
            {
                MessageBox.Show( "BAĞLANTINIZI KONTROL EDİNİZ...!!!");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                isaretci();   
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString(), "BAĞLANTI HATASI!");
            }
        }
        public int alinanid;
        public void isaretci()
        {
            listBox1.Items.Clear();
            vt.Baglantikapat();
            
            System.Data.SqlClient.SqlCommand komut = new System.Data.SqlClient.SqlCommand();
            komut.Connection = vt.bag;
            vt.bag.Open();
            komut.CommandText = "select hatid from vt_guzergah where durakid="+durakid+" and hatid in (Select hatid from vt_guzergah where durakid="+durakid2+")";
            //MessageBox.Show(durakid.ToString()+"   "+durakid2.ToString());
            System.Data.SqlClient.SqlDataReader dtr = komut.ExecuteReader();
            try
            {
                while (dtr.Read())
                {
                    alinanid = Convert.ToInt16(dtr["hatid"]);

                    vt.Baglantikapat();
                    System.Data.SqlClient.SqlCommand komut2 = new System.Data.SqlClient.SqlCommand();
                    komut2.Connection = vt.bag;
                    vt.bag.Open();
                    komut2.CommandText = "select * from vt_hatlar where hatid=" + alinanid;
                    System.Data.SqlClient.SqlDataReader dtr2 = komut2.ExecuteReader();
                    while (dtr2.Read())
                    {
                        listBox1.Items.Add(dtr2["hatadi"].ToString());
                    }
                }                                 
            }
            finally
            {
                vt.Baglantikapat();
            }
        }
        public void veriDoldur()
        {
            vt.Listegetir("select * from vt_duraklar where durakid=durakid");
            while (vt.sonuc.Read())
            {
                durak yenidurak = new durak(vt.sonuc["durakadi"].ToString(), Convert.ToSingle(vt.sonuc["enlem"]), Convert.ToSingle(vt.sonuc["boylam"]), Convert.ToInt16(vt.sonuc["durakid"]));
                //durakid = yenidurak.durakid;
                comboBox1.Items.Add(yenidurak);
                comboBox2.Items.Add(yenidurak);
            }
            vt.Baglantikapat();

            //vt.Listegetir("select * from vt_guzergah where guzergahid=guzergahid");
            //while (vt.sonuc.Read())
            //{
            //    guzergahid = Convert.ToInt16(vt.sonuc["guzergahid"]);
            //}
            //vt.Baglantikapat();
        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            //vt.Baglantikapat();
            /*if()
            {
                gmapOnClick gmo = new gmapOnClick();                

                System.Data.SqlClient.SqlDataReader dtr2 = vt.Listegetir("select * from vt_duraklar where durakid=" + durakid);
                while (dtr2.Read())
                {
                    ListViewItem lst = new ListViewItem(dtr2["durakadi"].ToString());
                    lst.SubItems.Add(dtr2["enlem"].ToString());
                    lst.SubItems.Add(dtr2["boylam"].ToString());
                    gmo.listView1.Items.Add(lst);
                }
                gmo.label1.Text = comboBox1.SelectedItem.ToString();
                gmo.ShowDialog();
            }
            else if (marker2.ToolTipText==comboBox2.SelectedItem.ToString()&& marker2.ToolTipText != comboBox1.SelectedItem.ToString())
            {
                gmapOnClick gmo = new gmapOnClick();                

                System.Data.SqlClient.SqlDataReader dtr2 = vt.Listegetir("select * from vt_duraklar where durakid=" + durakid2);
                while (dtr2.Read())
                {
                    ListViewItem lst = new ListViewItem(dtr2["durakadi"].ToString());
                    lst.SubItems.Add(dtr2["enlem"].ToString());
                    lst.SubItems.Add(dtr2["boylam"].ToString());
                    gmo.listView1.Items.Add(lst);
                    
                }
                gmo.label1.Text = comboBox2.SelectedItem.ToString();
                gmo.ShowDialog();
            }*/


        }
        bool cmb1Secim2 = false;
        bool cmb2Secim2 = false;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            durak secilidurak = (durak)comboBox1.SelectedItem;
            if (cmb1Secim2==false)
            {
                overlay.Markers.Remove(marker);
                //MessageBox.Show(secilidurak.enlem.ToString() + " : " + secilidurak.boylam.ToString());

                marker =
        new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
        new GMap.NET.PointLatLng(secilidurak.enlem, secilidurak.boylam),
        GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                overlay.Markers.Add(marker);
                gmap.Overlays.Add(overlay);
                marker.ToolTipText = comboBox1.SelectedItem.ToString();


                durakid = secilidurak.durakid;

                cmb1Secim2 = true;
            }
            else if(cmb1Secim2==true)
            {
                overlay.Markers.Remove(marker);
                marker =
        new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
        new GMap.NET.PointLatLng(secilidurak.enlem, secilidurak.boylam),
        GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                overlay.Markers.Add(marker);
                gmap.Overlays.Add(overlay);

                durakid = secilidurak.durakid;

                marker.ToolTipText = comboBox1.SelectedItem.ToString();
                cmb1Secim2 = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            durak secilidurak2 = (durak)comboBox2.SelectedItem;
            if (cmb2Secim2==false)
            {
                overlay.Markers.Remove(marker2);
                //MessageBox.Show(secilidurak.enlem.ToString() + " : " + secilidurak.boylam.ToString());
                marker2 =
                        new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                        new GMap.NET.PointLatLng(secilidurak2.enlem, secilidurak2.boylam),
                        GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                overlay.Markers.Add(marker2);
                gmap.Overlays.Add(overlay);
                marker2.ToolTipText = comboBox2.SelectedItem.ToString();
                durakid2 = secilidurak2.durakid;
                cmb2Secim2 = true;
            }
            else if(cmb2Secim2==true)
            {
                overlay.Markers.Remove(marker2);
                //MessageBox.Show(secilidurak.enlem.ToString() + " : " + secilidurak.boylam.ToString());
                marker2 =
                        new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                        new GMap.NET.PointLatLng(secilidurak2.enlem, secilidurak2.boylam),
                        GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                overlay.Markers.Add(marker2);
                gmap.Overlays.Add(overlay);
                marker2.ToolTipText = comboBox2.SelectedItem.ToString();
                durakid2 = secilidurak2.durakid;
                cmb2Secim2 = false;
            }
            
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            saatler st = new saatler();
            st.label2.Text = listBox1.SelectedItem.ToString();
            st.alinanid2 = alinanid;
            st.ShowDialog();            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Visible = true;            
        }
        System.Data.SqlClient.SqlCommand cmd;
        public void butunDuraklar()
        {
            gmap.Overlays.Clear();
            vt = new vt_islemleri();
            cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = vt.bag;
            cmd.CommandText = "select enlem,boylam from vt_duraklar where durakid in (select durakid from vt_guzergah where hatid="+alinanid+ ")";
            DataTable dtt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter adap = new System.Data.SqlClient.SqlDataAdapter(cmd);
            adap.Fill(dtt);
            GMap.NET.WindowsForms.GMapMarker marker12;
            foreach (PointLatLng p in dtt.Rows)
            {

                marker12 = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(p,GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                overlay.Markers.Add(marker12);

                //GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin
                //Add a marker on the overlay
                //markersOverlay.Markers.Add(marker1);
            }
            gmap.Overlays.Add(overlay);
            //gmap.Update();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            butunDuraklar();
        }
    }
}

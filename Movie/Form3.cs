using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace Movie
{
    public partial class Form3 : Form
    {
        public List<PictureBox> pictureBoxes = new List<PictureBox>();
        public int pic_count = 0;
        public List<Button> btn = new List<Button>();
        public int btn_count = 0;
        public List<TextBox> textBoxes = new List<TextBox>();
        public int text_count = 0;

        private Button currentButton;
        private Form activeForm;

        String retString;

        public Form3()
        {
            InitializeComponent();
            GetMoiveRank();
        }

        private void OpenChildForm(Form childForm, Object btnSender)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.Controls.Add(childForm);
            this.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void GetMoiveRank()  //movie_list.json 데이터 읽기
        {
            try
            {
                string url = "http://lazytitan.dothome.co.kr/movie_rank.json";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream);
                string retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                JsonParser(retString);

            }
            catch (Exception e)
            {
                //
            }


        }
        private void JsonParser(String json)
        {
            JArray array = JArray.Parse(json.ToString());
            int y = 10;
            int p_y = 10;
            int t_y = 10;

            foreach (JObject itemObj in array)
            {
                btn.Add(new Button());
                btn[btn_count] = new Button();
                this.Controls.Add(btn[btn_count]);
                btn[btn_count].Text = (itemObj["movie_name"].ToString());
                btn[btn_count].Location = new Point(565, y);
                btn[btn_count].Size = new Size(180, 50);

                pictureBoxes.Add(new PictureBox());
                pictureBoxes[pic_count] = new PictureBox();
                this.Controls.Add(pictureBoxes[pic_count]);
                pictureBoxes[pic_count].Location = new Point(25, y);
                pictureBoxes[pic_count].Size = new Size(94, 114);
                pictureBoxes[pic_count].ImageLocation = itemObj["count_img"].ToString();
                pictureBoxes[pic_count].Load(itemObj["count_img"].ToString());

                textBoxes.Add(new TextBox());
                textBoxes[text_count] = new TextBox();
                this.Controls.Add(textBoxes[pic_count]);
                textBoxes[text_count].Location = new Point(150, y);
                textBoxes[text_count].Size = new Size(300, 100);
                textBoxes[text_count].Multiline = true;
                textBoxes[text_count].Text += "제목 : " + itemObj["movie_name"].ToString() + "\r\n";
                textBoxes[text_count].Text += "\r\n순위 : " + itemObj["rank"].ToString() + "\r\n";
                textBoxes[text_count].Text += "변동률 : " + itemObj["variable"].ToString() + "\r\n";

                y += 150;
                p_y += 150;

                btn[btn_count].Click += button_Click;

                text_count++;
                pic_count++;
                btn_count++;
                //textBox1.Text += "제목 : " + itemObj["movie_name"].ToString() + "\r\n";
                //textBox1.Text += "순위 : " + itemObj["rank"].ToString() + "\r\n";
                //textBox1.Text += "변동률 : " + itemObj["variable"].ToString() + "\r\n\r\n";

            }
        }
        public void button_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Form4(), sender);
        }

    }
}

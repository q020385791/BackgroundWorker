using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bw=System.ComponentModel.BackgroundWorker;


namespace BackgroundWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initProgressBar();
            initbackgroundWorker();
        }
        private bw bw;
        private void btn_start_Click(object sender, EventArgs e)
        {
            //如果不是正在執行才做
            if (!bkWorker.IsBusy)
            {
                //避免多次點到
                this.btn_start.Text = "執行中";
                this.btn_start.Enabled = false;
                //進度條重置
                progressBar1.Value =0;
                bw.RunWorkerAsync();
            }
        }


        #region 初始化
        /// <summary>
        /// 初始化進度條
        /// </summary>
        public void initProgressBar()
        {
            progressBar1.Step = 1;
        }
        /// <summary>
        /// 初始化背景作業
        /// </summary>
        public void initbackgroundWorker()
        {
            bw = new bw();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            //下面掛上背景作業要做的事情
            //包括要執行的作業、完成後要做的事情、當執行修改的時候
            
            
            //要執行的作業
            bw.DoWork += new DoWorkEventHandler(bw_Dowork);

            //當作業完成後要做的事情
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_WorkerCompleted);

            bw.ProgressChanged += new ProgressChangedEventHandler(bw_Change);
        }
        #endregion

        /// <summary>
        /// 背景執行作業
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bw_Dowork(object sender, DoWorkEventArgs e)
        {
            

            for (int i = 1; i <= 10; i++)
            {
                //先判斷是否有點取消
                if (bw.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(700);
                    //觸發改變事件
                    bw.ReportProgress(i * 10);
                }
              
            }

        }

        /// <summary>
        /// 背景作業完成之後要做的事情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bw_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //判斷是不是被點取消

            if (e.Cancelled == true)
            {
                btn_start.Text = "已取消";
                //錯誤判斷
            } else if (e.Error!=null)
            {

                btn_start.Text = "錯誤："+e.Error.Message;
            }
            else
            {
                btn_start.Text = "完成作業";
            }
            
        }

        /// <summary>
        /// 觸發改變動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bw_Change(object sender,ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            //如果有設定可以中途終止的話
            if (bw.WorkerSupportsCancellation==true)
            {
                //取消該作業
                bw.CancelAsync();
                //避免多次點到
                this.btn_start.Text = "已取消";
            }
        }
    }
}

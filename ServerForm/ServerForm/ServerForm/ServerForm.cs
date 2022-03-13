using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerForm
{
    public partial class servertform : Form
    {
        //「netshの登録」の参考リンク
        //https://yryr.me/programming/csharp/httplistener-url-anything.html

        //コマンドプロンプト環境netshの実行(管理者権限）

        //URLの登録
        //netsh http add urlacl url=http://localhost:8080/ user=Everyone

        //URLの削除
        //netsh http delete urlacl url = http://localhost:8080/

        public servertform()
        {
            InitializeComponent();
        }

        bool StartButton_flg = false;
        HttpListener listener = null;

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton_flg == false)
            {
                StartButton.Text = "停止";
                StartButton_flg = true;
                await Task.Run(()=>{ ListenerStart("http://localhost:8080/"); });
            }
            else
            {
                StartButton.Text = "開始";
                StartButton_flg = false;
            }
        }

        public bool ListenerStart(string prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListenerがサポートされていません。");
                return false;
            }
            // URIプレフィックスが必要です。
            // for example "http://contoso.com:8080/".
            if (prefixes == null || prefixes.Length == 0)
            {
                return false;
            }

            //リスナーを作成します。
            if (listener == null)
            {
                listener = new HttpListener();

                // プレフィックスを追加します。
                listener.Prefixes.Add(prefixes);
            }

            listener.Start();
            Console.WriteLine("Listening...");

            // GetContextメソッドは、要求を待機している間はブロックします。
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // 応答オブジェクトを取得します。
            HttpListenerResponse response = context.Response;
            // 応答を作成します。
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // 応答ストリームを取得し、それに応答を書き込みます
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            // 出力ストリームを閉じる必要があります。
            output.Close();
            listener.Stop();
            return true;
        }
    }
}

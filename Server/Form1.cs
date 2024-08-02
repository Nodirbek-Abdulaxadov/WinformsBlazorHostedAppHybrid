namespace Server;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        webView21.Source = new Uri("http://localhost:7766");
    }
}
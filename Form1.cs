using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SorenChat
{
    public partial class SorenChatForm : Form
    {
        private const string AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjUwZTE3MjlkODE5MWZiNWRiZjY1NDZiMDA5ODkzOTlhNDEyYWZmZDhhYzBiMjc5Yjg0ZmJmYzczNzNiYzkzNmViMTYwNTQzMmI2MzVjMWFjIn0.eyJhdWQiOiIzNmI1N2RiODMyODhiMzc5MmUzODU3NDc0NTgxNzYwZSIsImp0aSI6IjUwZTE3MjlkODE5MWZiNWRiZjY1NDZiMDA5ODkzOTlhNDEyYWZmZDhhYzBiMjc5Yjg0ZmJmYzczNzNiYzkzNmViMTYwNTQzMmI2MzVjMWFjIiwiaWF0IjoxNzIxMDc4NzQ2LCJuYmYiOjE3MjEwNzg3NDYsImV4cCI6MTcyMTA4MjM0Niwic3ViIjoiIiwic2NvcGVzIjpbXSwidXNlciI6eyJpZCI6ODc1MjQ1NCwiZ3JvdXBfaWQiOm51bGwsInBhcmVudF9pZCI6bnVsbCwiY29udGV4dCI6eyJhY2NsaW0iOiIwIn0sImFyZWEiOiJyZXN0IiwiYXBwX2lkIjpudWxsfX0.tGH3UVijEl-V8vHI4H-yVFINpLixSezhFuoqIqmWPabIzij8fbGDncdNV800Dp4ksfIwdPbIKuo3ZzyK3NCrIJrwDajsRGUJpDyFTX5KQ-NovAyrqBsDRPzk_9eWIUbJW0xbsI3U37QaGwsP51ask6K0aa8po8SCtqkgx3L-q4eDz1DTHIWVNjRiRtyuDQiGlQeMCJ6xGmmqhbbE8H4WiIIjKinV3ncwVpj996HZ_yK1ccCoZsQnIICprsZXokLeal0Udm7kB-L_TaaOKBDlk4lCVhxTpA-liTLGY4xNE-Hocorxkgy0DRkOZIrVYoz9ZqKc3Y3-JdaehCz9J8yX1g"; // Substitua pelo seu token de acesso SendPulse
        private const string BaseUrl = "https://api.sendpulse.com/whatsapp";
        private readonly HttpClient client = new HttpClient();

        private TextBox messageTextBox;
        private ListBox conversationListBox;
        private Button sendMessageButton;
        private TextBox phoneNumberTextBox;
        private Button getContactButton;

        private System.ComponentModel.IContainer components;

        public SorenChatForm()
        {
            InitializeComponent();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        }

        private void InitializeComponent()
        {
            messageTextBox = new TextBox();
            conversationListBox = new ListBox();
            sendMessageButton = new Button();
            phoneNumberTextBox = new TextBox();
            getContactButton = new Button();
            SuspendLayout();

            // MessageTextBox
            messageTextBox.Location = new System.Drawing.Point(31, 71);
            messageTextBox.Name = "MessageTextBox";
            messageTextBox.Size = new System.Drawing.Size(200, 27);
            messageTextBox.TabIndex = 0;

            // ConversationListBox
            conversationListBox.Location = new System.Drawing.Point(31, 110);
            conversationListBox.Name = "ConversationListBox";
            conversationListBox.Size = new System.Drawing.Size(300, 264);
            conversationListBox.TabIndex = 1;

            // SendMessageButton
            sendMessageButton.Location = new System.Drawing.Point(250, 70);
            sendMessageButton.Name = "SendMessageButton";
            sendMessageButton.Size = new System.Drawing.Size(75, 30);
            sendMessageButton.TabIndex = 2;
            sendMessageButton.Text = "Enviar";
            sendMessageButton.Click += SendMessageButton_Click;

            // PhoneNumberTextBox
            phoneNumberTextBox.Location = new System.Drawing.Point(31, 30);
            phoneNumberTextBox.Name = "PhoneNumberTextBox";
            phoneNumberTextBox.Size = new System.Drawing.Size(200, 27);
            phoneNumberTextBox.TabIndex = 3;

            // GetContactButton
            getContactButton.Location = new System.Drawing.Point(250, 30);
            getContactButton.Name = "GetContactButton";
            getContactButton.Size = new System.Drawing.Size(75, 30);
            getContactButton.TabIndex = 4;
            getContactButton.Text = "Obter";
            getContactButton.Click += GetContactButton_Click;

            // SorenChatForm
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 500);
            Controls.Add(messageTextBox);
            Controls.Add(conversationListBox);
            Controls.Add(sendMessageButton);
            Controls.Add(phoneNumberTextBox);
            Controls.Add(getContactButton);
            Name = "SorenChatForm";
            Text = "Soren Chat";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Código de inicialização, se necessário
        }

        private async void SendMessageButton_Click(object sender, EventArgs e)
        {
            string message = messageTextBox.Text;
            conversationListBox.Items.Add("Você: " + message);
            messageTextBox.Clear();
            await SendToSendPulse(message);
        }

        private async void GetContactButton_Click(object sender, EventArgs e)
        {
            string phoneNumber = phoneNumberTextBox.Text;
            await GetContactInfo(phoneNumber);
        }

        private async Task SendToSendPulse(string message)
        {
            string botId = "66919e8eac493d6ceb0c080f"; // Substitua pelo ID do seu bot SendPulse

            var json = new StringContent(
                JsonSerializer.Serialize(new { bot_id = botId, phone = "+5511968580094", message = new { type = "text", text = new { body = message } } }),
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{BaseUrl}/contacts/sendByPhone", json);

            if (response.IsSuccessStatusCode)
            {
                conversationListBox.Items.Add("Soren: Mensagem enviada com sucesso!");
            }
            else
            {
                conversationListBox.Items.Add("Soren: Falha ao enviar mensagem. " + response.ReasonPhrase);
            }
        }

        private async Task GetContactInfo(string phoneNumber)
        {
            string botId = "66919e8eac493d6ceb0c080f"; // Substitua pelo ID do seu bot SendPulse

            var response = await client.GetAsync($"{BaseUrl}/contacts/getByPhone?phone=+5511968580094&bot_id={botId}");

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                using var jsonDocument = await JsonDocument.ParseAsync(contentStream);

                if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("variables", out JsonElement variablesElement))
                {
                    var respostaLista = variablesElement.GetProperty("respostaLista").GetString();
                    conversationListBox.Items.Add("Soren: " + respostaLista);
                }
                else
                {
                    conversationListBox.Items.Add("Soren: Resposta não encontrada.");
                }
            }
            else
            {
                conversationListBox.Items.Add("Soren: Erro ao obter informações: " + response.ReasonPhrase);
            }
        }
    }
}
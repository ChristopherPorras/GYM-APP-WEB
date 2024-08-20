using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace BL
{
    public class EmailManager
    {
        public async Task<Response> Execute(string emailRecipient)
        {
            var apiKey = "KEY";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jsolanoe@ucenfotec.ac.cr", "Empresa X");
            var subject = "Test correo";
            var to = new EmailAddress(emailRecipient, "Example User");
            var plainTextContent = "Esto es una prueba y yo soy poco creativo para escribir estos textos.";
            var htmlContent = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Template</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f4f4f4;\r\n        }\r\n        .email-container {\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .email-header {\r\n            background-color: #007bff;\r\n            color: #ffffff;\r\n            padding: 20px;\r\n            text-align: center;\r\n            border-radius: 8px 8px 0 0;\r\n        }\r\n        .email-header img {\r\n            max-width: 100px;\r\n            margin-bottom: 10px;\r\n        }\r\n        .email-content {\r\n            padding: 20px;\r\n            color: #333333;\r\n        }\r\n        .email-content h1 {\r\n            font-size: 24px;\r\n            margin-top: 0;\r\n        }\r\n        .email-content p {\r\n            font-size: 16px;\r\n            line-height: 1.5;\r\n        }\r\n        .email-footer {\r\n            background-color: #f1f1f1;\r\n            color: #555555;\r\n            padding: 20px;\r\n            text-align: center;\r\n            border-radius: 0 0 8px 8px;\r\n        }\r\n        .email-footer a {\r\n            color: #007bff;\r\n            text-decoration: none;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"email-container\">\r\n        <div class=\"email-header\">\r\n            <img src=\"https://via.placeholder.com/100\" alt=\"Company Logo\">\r\n            <h2>Company Name</h2>\r\n        </div>\r\n        <div class=\"email-content\">\r\n            <h1>Welcome to Our Service</h1>\r\n            <p>Dear [Recipient's Name],</p>\r\n            <p>Thank you for choosing our service. We are thrilled to have you on board. Our goal is to provide you with the best experience possible. Below are the details of your account and services:</p>\r\n            <p><strong>Account Details:</strong></p>\r\n            <p>Username: [Username]</p>\r\n            <p>Email: [Recipient's Email]</p>\r\n            <p><strong>Service Details:</strong></p>\r\n            <p>Service Plan: [Service Plan]</p>\r\n            <p>Start Date: [Start Date]</p>\r\n            <p>We hope you enjoy using our service. If you have any questions, feel free to reach out to our support team.</p>\r\n            <p>Best regards,</p>\r\n            <p>The [Company Name] Team</p>\r\n        </div>\r\n        <div class=\"email-footer\">\r\n            <p>&copy; 2024 Company Name. All rights reserved.</p>\r\n            <p><a href=\"#\">Unsubscribe</a> | <a href=\"#\">Privacy Policy</a></p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}

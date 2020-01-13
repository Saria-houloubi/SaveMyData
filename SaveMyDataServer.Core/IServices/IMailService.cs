using SaveMyDataServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyDataServer.Core.IServices
{
    public interface IMailService
    {
        /// <summary>
        /// Sends the email to the user with the sent information
        /// </summary>
        /// <param name="emailModel">The email information to fill it with</param>
        /// <returns></returns>
        Task<bool> SendEmail(EmailModel emailModel);
    }
}

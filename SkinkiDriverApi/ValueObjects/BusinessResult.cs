using SkinkiDriverApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkinkiDriverApi.ValueObjects
{
    /// <summary>
    /// Classe usada para transitar entre serviços e controller
    /// </summary>    
    public class BusinessResult<T>
    {
        #region Constructor

        public BusinessResult()
        {
            Messages = new List<MessageResult>();
        }

        public BusinessResult(T data)
        {
            Data = data;
            Messages = new List<MessageResult>();
        }

        #endregion

        public T Data { get; set; }
        public List<MessageResult> Messages { get; set; }
        public string Token { get; set; }

        public bool IsValid
        {
            get
            {
                return Messages == null || !Messages.Any(x => x.Type == MessageType.Error);
            }
        }

        #region Factory

        public static BusinessResult<T> CreateValidResult(T model)
        {
            return new BusinessResult<T>(model);
        }

        public static BusinessResult<T> CreateValidResult()
        {
            return new BusinessResult<T>(default);
        }

        public static BusinessResult<T> CreateInvalidResult(string message)
        {
            var result = new BusinessResult<T>(default);
            result.Messages = AddError(message);

            return result;
        }

        public static BusinessResult<T> CreateInvalidResult(T model, string message)
        {
            var result = new BusinessResult<T>();
            result.Data = model;
            result.Messages = AddError(message);

            return result;
        }

        public static BusinessResult<T> CreateInvalidResult(List<MessageResult> message)
        {
            var bo = new BusinessResult<T>
            {
                Messages = message
            };
            return bo;
        }

        public static BusinessResult<T> CreateInvalidResult(T model, string message, Exception exception = null)
        {
            var result = new BusinessResult<T>();
            result.Data = model;
            result.Messages = AddError(message, exception);

            return result;
        }

        public static BusinessResult<T> CreateInvalidResult(Exception exception = null)
        {
            var result = new BusinessResult<T>();
            result.Messages = AddError(exception);

            return result;
        }

        public static BusinessResult<T> CreateInvalidResult(IEnumerable<string> errors)
        {
            var result = new BusinessResult<T>();
            result.Messages = AddError(errors);
            return result;
        }

        public void WithErrors(params string[] errors)
        {
            if (errors != null && errors.Any())
            {
                Messages.AddRange(AddError(errors));
            }
        }

        public void WithErrors(List<MessageResult> errors)
        {
            if (errors != null && errors.Any())
            {
                Messages.AddRange(errors);
            }
        }

        public void WithErrors(MessageResult errors)
        {
            if (errors != null)
            {
                Messages.Add(errors);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageError">Mensagem de erro</param>
        /// <param name="typeCustom">Tipo customizado em HTML para usar no frontend</param>
        public void WithErrors(string messageError, string typeCustom)
        {
            if (!string.IsNullOrEmpty(messageError))
            {
                Messages.Add(new MessageResult()
                {
                    Message = messageError,
                    Type = MessageType.Error,
                    TypeCustom = typeCustom
                });
            }
        }

        public void WithErrors(Exception exception)
        {
            if (exception != null)
            {
                Messages.AddRange(AddError(exception.Message));
            }
        }

        public void WithSucess(params string[] message)
        {
            if (message != null && message.Any())
            {
                Messages.AddRange(AddSucess(message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Mensagem de sucesso</param>
        /// <param name="typeCustom">Usado para algo em HTML no frontend</param>
        public void WithSucess(string message, string typeCustom = null)
        {
            if (message != null && message.Any())
            {
                Messages.AddRange(AddSucess(message, typeCustom));
            }
        }

        private static List<MessageResult> AddSucess(string message, string typeCustom = null)
        {
            var messageResult = new List<MessageResult>();
            messageResult.Add(new MessageResult()
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Success,
                TypeCustom = typeCustom
            });
            return messageResult;
        }

        private static List<MessageResult> AddSucess(IEnumerable<string> message)
        {
            var messageResult = new List<MessageResult>();
            foreach (var item in message)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = item,
                    Type = MessageType.Success
                });
            }
            return messageResult;
        }

        private static List<MessageResult> AddError(string message, Exception exception = null)
        {
            var messageResult = new List<MessageResult>();
            messageResult.Add(new MessageResult()
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Error
            });
            if (exception != null)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error
                });
            }
            return messageResult;
        }

        private static List<MessageResult> AddError(Exception exception = null)
        {
            var messageResult = new List<MessageResult>();
            if (exception != null)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error
                });
            }
            return messageResult;
        }

        private static List<MessageResult> AddError(IEnumerable<string> errors, Exception exception = null)
        {
            var messageResult = new List<MessageResult>();
            foreach (var item in errors)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = item,
                    Type = MessageType.Error
                });
            }

            if (exception != null)
            {
                messageResult.Add(new MessageResult()
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error
                });
            }
            return messageResult;
        }

        #endregion
    }
}

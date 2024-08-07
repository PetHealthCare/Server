﻿using BusinessObjects.Models;
using DTOs.Request.Payment;
using DTOs.Response.Payment;
using Repositories;
using Repositories.Interface;
using Services.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPaymentService
    {
        List<PaymentResponse> GetPayments(GetListPaymentRequest request);
        PaymentResponse GetPaymentById(int id);
        Payment AddPayment(PaymentRequest payment);
        bool UpdatePayment(PaymentRequest payment);
        bool DeletePayment(int id);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionRepository _tranRepo;

		public PaymentService(IPaymentRepository paymentRepository, ITransactionRepository tranRepo)
		{
			_paymentRepository = paymentRepository;
			_tranRepo = tranRepo;
		}

		public List<PaymentResponse> GetPayments(GetListPaymentRequest request)
        {

            var payments = _paymentRepository.GetPayments().AsQueryable();
            if (request.BillId > 0)
            {
                payments = payments.Where(x => x.BillId == request.BillId);
            }
            var response = new List<PaymentResponse>();
            foreach (var payment in payments)
            {
                response.Add(new PaymentResponse
                {
                    PaymentId = payment.PaymentId,
                    Amount = payment.Amount,
                    Method = payment.Method,
                    InsDate = payment.InsDate,
                    Status = payment.Status,
                    BillId = payment.BillId
                    
                });
            }
            return response;
        }
        public PaymentResponse GetPaymentById(int id)
        {
            var payment = _paymentRepository.GetPaymentById(id);
            if (payment == null)
            {
                return null;
            }
            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                Method = payment.Method,
                InsDate = payment.InsDate,
                Status = payment.Status,
                BillId = payment.BillId
            };
            
        }
        public Payment AddPayment(PaymentRequest payment)
        {
            var request = new Payment
            {
                Amount = payment.Amount,
                Method = payment.Method,
                InsDate = Utils.GetDateTimeNow(),
                Status = payment.Status,
                BillId = payment.BillId
            };
            var paymenCreated = _paymentRepository.AddPayment(request);
			var transaction = new Transaction
            {
                Amount = payment.Amount,
                BillId = payment.BillId,
                TransactionDate = Utils.GetDateTimeNow(),
                TransactionId = paymenCreated.PaymentId,
			};
            _tranRepo.Create(transaction);
            return paymenCreated;
        }

        public bool UpdatePayment(PaymentRequest payment)
        {
            var request = new Payment
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                Method = payment.Method,
                InsDate = Utils.GetDateTimeNow(),
                Status = payment.Status,
                BillId = payment.BillId
            };
			var transaction = new Transaction
			{
				Amount = request.Amount,
				BillId = request.BillId,
				TransactionDate = Utils.GetDateTimeNow(),
				TransactionId = request.BillId,
			};
			_tranRepo.Create(transaction);

			return _paymentRepository.UpdatePayment(request);
        }
        public bool DeletePayment(int id)
        {
            var payment = _paymentRepository.GetPaymentById(id);
            if (payment == null)
            {
                return false;
            }
            return _paymentRepository.DeletePayment(id);
        }
    }
}

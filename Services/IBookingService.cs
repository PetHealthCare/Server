﻿using AutoMapper;
using BusinessObjects.Models;
using DTOs.Request.Booking;
using DTOs.Request.Schedule;
using DTOs.Response.Booking;
using DTOs.Response.Pet;
using Repositories;
using Services.Extentions.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookingService
    {
        
        public Task<Booking> CreateBooking(BookingRequest booking);
        public Task<bool> UpdateStatusBooking(int bookingId);
        public Booking GetBookingById(int booking);
        public Task<bool> CreateSlotBookingAndLinkToBooking(CreateScheduleAndSlotBookingRequest createScheduleAndSlot); 
    }
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISlotBookingRepository _slotBookingRepository;

        public BookingService(IBookingRepository repo, IMapper mapper, IScheduleRepository scheduleRepository, ISlotBookingRepository slotBookingRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _scheduleRepository = scheduleRepository;
            _slotBookingRepository = slotBookingRepository;
        }

        public async Task<Booking> CreateBooking(BookingRequest request)
        {
            Booking booking = new Booking();
            booking.PetId = request.PetId;
            booking.CustomerId = request.CustomerId;
            booking.BookingDate = DateTime.Now;
            booking.Note = request.Note;
            booking.Status = false;
            return await _repo.CreateBooking(booking);
        }

        public async Task<bool> CreateSlotBookingAndLinkToBooking(CreateScheduleAndSlotBookingRequest createScheduleAndSlot)
        {
            var bookingResponse = _repo.GetBookingById(createScheduleAndSlot.bookingId);
            if (bookingResponse == null)
            {
                throw new Exception("Booking not found");
            }
            ScheduleRequest scheduleRequest = new ScheduleRequest();
            scheduleRequest.StartTime = createScheduleAndSlot.StartTime;
            scheduleRequest.EndTime = createScheduleAndSlot.EndTime;
            
            var scheduleRespone = await _scheduleRepository.Create(scheduleRequest);
            if (scheduleRespone == null)
            {
                throw new Exception("Failed to create schedule");
            }
            SlotBookingRequest slotBookingRequest = new SlotBookingRequest();
            slotBookingRequest.DoctorId = createScheduleAndSlot.DoctorId;
            slotBookingRequest.ServiceId = createScheduleAndSlot.ServiceId;
            slotBookingRequest.ScheduleId = scheduleRespone.ScheduleId;
           
            var slotBookingResponse = await _slotBookingRepository.Create(slotBookingRequest);

            if (slotBookingResponse == null)
            {
                throw new Exception("Failed to create slot booking");
            }
            var Response = await _repo.LinkBookingToSlotBooking(createScheduleAndSlot.bookingId, slotBookingResponse.SlotBookingId);
            if(Response == false)
            {
                throw new Exception("Failed to create slot booking");
            }
            var updateStatusResponse = await _repo.UpdateStatusBooking(createScheduleAndSlot.bookingId);
            return true;  
         }

        public Booking GetBookingById(int booking)
        {
           return _repo.GetBookingById(booking);
        }

      

        public  async Task<bool> UpdateStatusBooking(int bookingId)
        {
            return await _repo.UpdateStatusBooking(bookingId);
        }


    }
}
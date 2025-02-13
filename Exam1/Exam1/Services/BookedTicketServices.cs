using Azure.Core;
using Exam1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using Ticket.Entities;

namespace Exam1.Services
{
    public class BookedTicketServices
    {
        public readonly Exam1Context _db;
        public BookedTicketServices(Exam1Context db)
        {
            _db = db;
        }

        //untuk cek semua data
        public async Task<List<GetAllBookedCategoryModel>> Get1()
        {

            var datas = await _db.BookedTickets
                .GroupBy(bt => bt.CategoryName.Trim())
                .Select(Q => new GetAllBookedCategoryModel
                {
                    qtyPerCategory = Q.Sum(bt => bt.Quota),
                    bookedCategoryName = Q.Key,
                    bookedTickets = Q.Select(bt => new GetAllSimpleBookedTicketModel
                    {
                        bookedTicketCode = bt.TicketCode,
                        bookedTicketName = bt.TicketName,
                        bookedSeat = bt.Seat,
                        bookedEventDate = bt.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        id = bt.BookedId
                    }).ToList()
                }).ToListAsync();

            if (datas == null || !datas.Any())
            {
                return null;
            }

            return datas;

        }

        public async Task<List<GetBookedCategoryModel>> Get(int id)
        {

            var datas = await _db.BookedTickets
                .Where(bt => bt.BookedId == id)
                .GroupBy(bt => bt.CategoryName.Trim())
                .Select(Q => new GetBookedCategoryModel
                {
                    qtyPerCategory = Q.Sum(bt => bt.Quota),
                    bookedCategoryName = Q.Key,
                    bookedTickets = Q.Select(bt => new GetSimpleBookedTicketModel
                    {
                        bookedTicketCode = bt.TicketCode,
                        bookedTicketName = bt.TicketName,
                        bookedSeat = bt.Seat,
                        bookedEventDate = bt.EventDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    }).ToList()
                }).ToListAsync();

            if(datas == null || !datas.Any())
            {
                return null;
            }

            return datas;

        }

        public async Task<string> Post(PostBookedTicketDTO request)
        {

            foreach (var category in request.ticketPerCategory)
            {
                foreach (var ticket in category.bookedTickets)
                {
                    var newData = new BookedTicket
                    {
                        EventDate = DateTime.Now,
                        TicketCode = ticket.bookedTicketCode,
                        TicketName = ticket.bookedTicketName,
                        CategoryName = category.bookedCategoryName,
                        Seat = ticket.bookedSeat,
                        Price = ticket.bookedPrice,
                        Quota = ticket.quantity
                    };

                    _db.BookedTickets.Add(newData);

                    var availTicket = await _db.AvailableTickets
                        .FirstOrDefaultAsync(a => a.TicketCode == ticket.bookedTicketCode);

                    if (availTicket == null)
                    {
                        return null;
                    }
                    
                    if (DateTime.Now >= availTicket.EventDate)
                    {
                        return "Sudah Expired";
                    }

                    if (availTicket.Quota < ticket.quantity)
                    {
                        return "Tidak Cukup";
                        
                    }

                    if(availTicket.Quota == 0)
                    {
                        return "Habis";
                    }
                    availTicket.Quota -= ticket.quantity;
                }
            }

            await _db.SaveChangesAsync();

            return "success";

            //var groupedData = await _db.BookedTickets
            //    .GroupBy(bt => bt.CategoryName)
            //    .Select(Q => new PostBookedCategoryModel
            //    {
            //        bookedCategoryName = Q.Key,
            //        summaryPrice = Q.Sum(bt => bt.Price),
            //        bookedTickets = Q.Select(bt => new PostSimpleBookedTicketModel
            //        {
            //            bookedTicketCode = bt.TicketCode,
            //            bookedTicketName = bt.TicketName,
            //            bookedPrice = bt.Price,
            //            bookedSeat = bt.Seat,
            //        }).ToList()
            //    }).ToListAsync();

            //var totalPrice = groupedData.Sum(c => c.summaryPrice);

            //return new PostBookedTicketDTO
            //{
            //    summaryPrice = totalPrice,
            //    ticketPerCategory = groupedData
            //};
        }

        public async Task<PutBookedTicketRemainsListModel> Update(int id, PutSimpleBookedTicketModel request)
        {
            var isIdExist = await _db.BookedTickets
                .Where(Q => Q.BookedId == id)
                .FirstOrDefaultAsync();

            var dto = new PutBookedTicketRemainsListModel();

            if (isIdExist == null)
            {
                dto.message = "Id tidak ada";
                return dto; 
            }

            var isCodeExist = await _db.BookedTickets
                .Where(Q => Q.TicketCode == request.bookedTicketCode)
                .FirstOrDefaultAsync();

            if (isCodeExist == null)
            {
                dto.message = "Code tidak ada";
                return dto; 
            }

            if(request.quantity < 1)
            {
                dto.message = "Quantity minimal satu"; 
                return dto;
            }

            //memanipulasi jumlah quantity pada AvailTicket
            int oldQuota = isIdExist.Quota; //bookedTicket
            int newQuota = request.quantity; //user input
            int quotaDiff = oldQuota - newQuota; //berapa bedanya

            var availTicket = await _db.AvailableTickets
                .Where(Q => Q.TicketCode == request.bookedTicketCode)
                .FirstOrDefaultAsync();

            if(availTicket == null || request.quantity > availTicket.Quota)
            {
                dto.message =  "Quantity tidak boleh lebih dari sisah";
                return dto;
            }

            if(newQuota > oldQuota)
            {
                availTicket.Quota -= Math.Abs(quotaDiff);
            }

            if(newQuota < oldQuota)
            {
                availTicket.Quota += Math.Abs(quotaDiff);
            }

            isIdExist.TicketCode = request.bookedTicketCode;
            isIdExist.Quota = request.quantity;

            await _db.SaveChangesAsync();

            var updatedDatas = await _db.BookedTickets
                .Select(Q => new PutBookedTicketDTO
                {
                    bookedTicketCode = Q.TicketCode,
                    bookedTicketName = Q.TicketName,
                    quantity = Q.Quota,
                    bookedCategoryName = Q.CategoryName
                }).ToListAsync();

            return new PutBookedTicketRemainsListModel
            {
                message = "Success",
                putDTO = updatedDatas
            };

        }

        public async Task<DeleteBookedTicketRemainsListModel> Delete(int id, string ticketCode, int quantity)
        {
            var existingData = await _db.BookedTickets
                .Where(Q => Q.BookedId == id && Q.TicketCode == ticketCode)
                .FirstOrDefaultAsync();

            var response = new DeleteBookedTicketRemainsListModel();

            if (existingData == null)
            {
                response.errorMessage = "Ticket/Id salah";
                return response;

            }
            
            if(existingData.Quota < quantity)
            {
                response.errorMessage = "Quantity over";
                return response;
            }

            existingData.Quota -= quantity;

            if(existingData.Quota == 0)
            {
                _db.BookedTickets.Remove(existingData);
            }


            var ticketRemains = await _db.BookedTickets
                .Where(Q => Q.BookedId == id && Q.Quota > 0)
                .ToListAsync();

            if (ticketRemains.Count == 0)
            {

                var data = await _db.BookedTickets
                    .Where(Q => Q.BookedId == id)
                    .FirstOrDefaultAsync();

                if (data != null)
                {
                    _db.BookedTickets.Remove(data);
                }
            }

            await _db.SaveChangesAsync();

            var dataRemains = await _db.BookedTickets
                .Where(Q =>  Q.Quota > 0)
                .ToListAsync();

            var remaining = dataRemains.Select(ticket => new DeleteBookedTicketRemainsModel
            {
                ticketCode = ticket.TicketCode,
                ticketName = ticket.TicketName,
                quantity = ticket.Quota,  
                categoryName = ticket.CategoryName 
            }).ToList();


            return new DeleteBookedTicketRemainsListModel
            {
                errorMessage = "Success",
                remainModels = remaining
            };

        }
    }
}

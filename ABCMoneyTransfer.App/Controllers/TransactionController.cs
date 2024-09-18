using ABCMoneyTransfer.Data.DTOs;
using ABCMoneyTransfer.Data.Entities;
using ABCMoneyTransfer.Data.Repositories;
using ABCMoneyTransfer.Data.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ABCMoneyTransfer.App.Controllers
{
    public class TransactionController(ITransactionRepository transactionRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<TransactionController> logger)
        : Controller

    {

        [Route("transaction/list")]
        public IActionResult List()
        {
            return View();
        }

        [Route("transaction/new")]
        public IActionResult New()
        {
            return View();
        }


        [HttpPost , Route("api/transaction/insert")]
        public async Task<IActionResult> Insert([FromBody] TransactionInsertDto transactionDto)
        {
            try
            {
                Transaction transaction = mapper.Map<Transaction>(transactionDto);
                transaction.CreatedDate = GeneralUtility.GetCurrentNepaliDateTime();
                transaction.CreatedBy = GeneralUtility.GetUsernameFromClaim(httpContextAccessor);
                await transactionRepository.Insert(transaction);
                return Ok("Transaction has been completed");
            }
            catch (Exception e)
            {
                logger.LogError(e , $"{nameof(TransactionController)}- {nameof(Insert)}");
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Route("api/transactions")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Transaction> transactions = await transactionRepository.GetAll();
                IEnumerable<TransactionDto> mappedTransactions = mapper.Map<IEnumerable<TransactionDto>>(transactions);
                return Ok(mappedTransactions);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

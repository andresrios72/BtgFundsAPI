using BtgFundsApi.DTOs;
using BtgFundsApi.Exceptions;
using BtgFundsApi.Models;
using BtgFundsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BtgFundsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FundsController : ControllerBase
    {
        private readonly IFundService _fundService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;

        public FundsController(IFundService fundService, IUserService userService, ITransactionService transactionService)
        {
            _fundService = fundService;
            _userService = userService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Fund>>> Get() =>
            await _fundService.GetAsync();

        // endpoint de suscripción
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeFundRequest request)
        {
            // Validar usuario
            var user = await _userService.GetByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {request.UserId} no encontrado.");

            // Validar fondo
            var fundList = await _fundService.GetAsync();
            var fund = fundList.FirstOrDefault(f => f.Id == request.FundId);
            if (fund == null)
                throw new NotFoundException("El fondo solicitado no existe.");

            // Validar monto mínimo
            if (request.Amount < fund.MinAmount)
                return BadRequest($"El monto mínimo para este fondo es {fund.MinAmount}.");

            // Validar saldo
            if (user.Balance < request.Amount)
                throw new BusinessRuleException("No tiene saldo suficiente para realizar esta suscripción al fondo {fund.Name}.");

            // Actualizar usuario
            user.Balance -= request.Amount;
            user.FundsSubscribed.Add(new UserFundSubscription
            {
                FundId = fund.Id,
                SubscriptionDate = DateTime.UtcNow,
                Amount = request.Amount
            });
            await _userService.UpdateAsync(user.Id, user);

            // Registrar transacción
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FundId = fund.Id,
                Type = "subscription",
                Amount = request.Amount,
                Date = DateTime.UtcNow
            };
            await _transactionService.CreateAsync(transaction);

            // Simular notificación
            Console.WriteLine($"[Notificación] Usuario {user.Name} se ha suscrito a {fund.Name} por {request.Amount}. Preferencia: {user.NotificationPreference}");

            return Ok($"Suscripción realizada exitosamente a {fund.Name} por {request.Amount}.");
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionRequest request)
        {
            // Obtener usuario
            var user = await _userService.GetByIdAsync(request.UserId);
            if (user == null)
                return NotFound($"Usuario con ID {request.UserId} no encontrado.");

            // Validar que el usuario esté suscrito al fondo
            var subscription = user.FundsSubscribed.FirstOrDefault(s => s.FundId == request.FundId);
            if (subscription == null)
                return BadRequest($"El usuario no tiene suscripción activa en el fondo con ID {request.FundId}.");

            // Devolver saldo
            user.Balance += subscription.Amount;

            // Remover la suscripción
            user.FundsSubscribed.Remove(subscription);
            await _userService.UpdateAsync(user.Id, user);

            // Registrar transacción
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FundId = request.FundId,
                Type = "cancellation",
                Amount = subscription.Amount,
                Date = DateTime.UtcNow
            };
            await _transactionService.CreateAsync(transaction);

            // Simular notificación
            Console.WriteLine($"[Notificación] Usuario {user.Name} ha cancelado su suscripción al fondo {request.FundId} por {subscription.Amount}. Preferencia: {user.NotificationPreference}");

            return Ok($"Cancelación realizada exitosamente y se han devuelto {subscription.Amount} al usuario.");
        }

    }
}

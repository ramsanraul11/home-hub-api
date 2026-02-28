using System.Runtime.Intrinsics.X86;

namespace HomeHub.Application.Inventory.Commands.UpdateItemQuantity
{
    public sealed class UpdateItemQuantityHandler
    {
        private readonly IInventoryRepository _repo;

        public UpdateItemQuantityHandler(IInventoryRepository repo)
            => _repo = repo;

        public async Task<Result<bool>> Handle(
            Guid householdId,
            Guid itemId,
            Guid userId,
            UpdateItemQuantityCommand cmd,
            CancellationToken ct)
        {
            if (cmd.Amount <= 0)
                return Result<bool>.Fail("inventory.amount_invalid", "Amount must be greater than 0.");

            var item = await _repo.GetItemAsync(householdId, itemId, ct);
            if (item is null)
                return Result<bool>.Fail("inventory.item_not_found", "Item not found.");

            var wasLowStock = item.IsLowStock();

            if (cmd.Operation == QuantityOperation.Add)
                item.Add(cmd.Amount, userId);
            else
                item.Consume(cmd.Amount, userId);

            await _repo.SaveChangesAsync(ct);

            var isNowLowStock = item.IsLowStock();

            // Evento simple: pasó a low stock
            //Ese bool que devolvemos(triggeredLowStockEvent) es el punto perfecto para:

            //Crear LowStockAlert

            //Crear Notification

            //Enviar push

            //Enviar email

            //Pero lo hacemos en Fase 4.3 / Fase 5.

            //Ahora mismo ya tienes:

            //✔ Categorías
            //✔ Items
            //✔ Filtros
            //✔ Stock mínimo
            //✔ Evento cuando baja de mínimo

            //Arquitectura limpia.Sin dependencias cruzadas.
            var triggeredLowStockEvent = !wasLowStock && isNowLowStock;

            return Result<bool>.Ok(triggeredLowStockEvent);
        }
    }
}

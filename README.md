## Roadmap (MVP → v1)

### Fase 0 — Base técnica (Done/Now)
- [x] Solución por capas (Api / Application / Infrastructure / Domain)
- [x] PostgreSQL + EF Core
- [x] Identity (Guid) + JWT
- [x] Refresh tokens (rotación + hash en BD)
- [x] Swagger + CORS dev

### Fase 1 — Core Hogar (MVP)
- [ ] Households: crear hogar + listar mis hogares
- [ ] Members: añadir miembros (manual) + roles (Owner/Admin/Member)
- [ ] Autorización por hogar (policy “IsHouseholdMember”)
- [ ] Seed de datos mínimo (opcional)

### Fase 2 — Tareas del hogar (MVP)
- [ ] CRUD TaskItem (por hogar)
- [ ] Asignaciones (TaskAssignment)
- [ ] Completar / reabrir tareas
- [ ] Filtros (estado, vencimiento, asignado)
- [ ] Auditoría básica (CreatedAt/By)

### Fase 3 — Avisos (MVP)
- [ ] CRUD Notice (por hogar)
- [ ] Archivado + filtros por urgencia/fecha

### Fase 4 — Compra/Inventario (MVP)
- [ ] Inventory: categorías + items + stock mínimo
- [ ] ShoppingList: listas + items + marcar comprado
- [ ] Alertas de stock bajo (evento → notificación)

### Fase 5 — Notificaciones (v1)
- [ ] Modelo Notifications + Deliveries
- [ ] Web Push / Email (fase 1)
- [ ] WhatsApp (fase 2, API oficial)

### Fase 6 — Finanzas (v1+)
- [ ] Categorías de gasto + transacciones manuales
- [ ] Gastos fijos (recurrencias)
- [ ] Integración bancaria (Tink/TrueLayer) + sync background
- [ ] Dashboard económico del hogar

### Calidad / DevEx (continuo)
- [ ] Tests unitarios (Domain/Application)
- [ ] Integration tests (Api + Db container)
- [ ] CI (GitHub Actions)
- [ ] Observabilidad (logs + trazas)
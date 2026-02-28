## Roadmap (MVP → v1)

### Fase 0 — Base técnica (Done/Now)
- [x] Solución por capas (Api / Application / Infrastructure / Domain)
- [x] PostgreSQL + EF Core
- [x] Identity (Guid) + JWT
- [x] Refresh tokens (rotación + hash en BD)
- [x] Swagger + CORS dev

### Fase 1 — Core Hogar (MVP)
- [x] Households: crear hogar + listar mis hogares
- [x] Members: añadir miembros (manual) + roles (Owner/Admin/Member)
- [x] Autorización por hogar (policy “IsHouseholdMember”)
- [x] Seed de datos mínimo (opcional)
- [x] GET /households/{householdId}/members (lista miembros + roles)
- [x] PATCH /households/{householdId}/members/{memberId} (cambiar rol)
- [x] DELETE /households/{householdId}/members/{memberId} (expulsar)

### Fase 2 — Tareas del hogar (MVP)
- [x] CRUD TaskItem (por hogar)
- [x] Asignaciones (TaskAssignment)
- [x] Completar
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
using ef_core.Data;

using var contexto = new ExemploContext();

contexto.ChangeTracker.Clear();

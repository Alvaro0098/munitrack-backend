using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IIncidenceRepository _incidenceRepository;

        public OperatorService(IOperatorRepository operatorRepository, IIncidenceRepository incidenceRepository)
        {
            _operatorRepository = operatorRepository;
            _incidenceRepository = incidenceRepository;
        }

        public List<Operator> GetOperators()
        {
            return _operatorRepository.Get();
        }
        public void CreateOperator(CreateOperatorDto Dto)
        {
            // Validación: Verificar duplicado de DNI EN OPERADORES ACTIVOS
            // Permite reutilizar DNI de operadores eliminados
            var existingOperator = _operatorRepository.GetOperatorByDni(Dto.DNI);
            if (existingOperator != null)
                throw new InvalidOperationException($"Ya existe un operador activo con DNI {Dto.DNI}");

            // Validación: Verificar duplicado de N° de Legajo EN OPERADORES ACTIVOS
            // Permite reutilizar NLegajo de operadores eliminados
            var existingByLegajo = _operatorRepository.GetOperatorByNLegajo(Dto.NLegajo);
            if (existingByLegajo != null)
                throw new InvalidOperationException($"Ya existe un operador activo con N° de Legajo {Dto.NLegajo}");

            try
            {
                Operator newOperator = new Operator()
                {
                    DNI = Dto.DNI,
                    Name = Dto.Name,
                    LastName = Dto.LastName,
                    NLegajo = Dto.NLegajo,
                    Password = Dto.Password,
                    Phone = Dto.Phone,
                    Email = Dto.Email,
                    Position = Dto.Position,
                    Deleted = 0
                };
                _operatorRepository.AddOperator(newOperator);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar el operador. {ex.Message}");
            }
        }

        public bool DeleteOperator(int dni)
        {
            var operatorDelete = _operatorRepository.GetOperatorByDni(dni);
            if (operatorDelete is null)
                return false;

            // Validar que no haya incidencias activas vinculadas
            var linkedIncidences = _incidenceRepository.GetIncidencesByOperatorId(operatorDelete.NLegajo);
            if (linkedIncidences.Count > 0)
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar el operador '{operatorDelete.Name} {operatorDelete.LastName}' porque tiene {linkedIncidences.Count} incidencia(s) activa(s) vinculada(s)"
                );
            }
            
            try
            {
                operatorDelete.Deleted = 1;
                _operatorRepository.UpdateOperator(operatorDelete);
                return true;
            }
            catch (Exception ex)
            {
                // 🎯 REGLA DE ORO: No pasar ex como parámetro. Solo mensaje limpio.
                Console.WriteLine($"Exception en DeleteOperator: {ex.GetType().Name} - {ex.Message}");
                throw new InvalidOperationException("Error al eliminar el operador. Intente nuevamente.");
            }
        }

        public Operator? GetOperatorByDni(int dni)
        {
            return _operatorRepository.GetOperatorByDni(dni);
        }

        public Operator UpdateOperator(int dni, UpdateOperatorDto Dto)
        {
            var operatorEntity = _operatorRepository.GetOperatorByDni(dni);
            if (operatorEntity is null)
                throw new InvalidOperationException("Operador no encontrado.");

            try
            {
                operatorEntity.Name = Dto.Name;
                operatorEntity.LastName = Dto.LastName;
                // Solo actualizar password si se proporciona uno (no vacío)
                if (!string.IsNullOrWhiteSpace(Dto.Password))
                {
                    operatorEntity.Password = Dto.Password;
                }
                operatorEntity.Phone = Dto.Phone;
                operatorEntity.Email = Dto.Email;
                operatorEntity.Position = Dto.Position;

                _operatorRepository.UpdateOperator(operatorEntity);
                return operatorEntity;
            }
            catch (Exception ex)
            {
                // 🎯 REGLA DE ORO: No pasar ex como parámetro. Solo mensaje limpio.
                Console.WriteLine($"Exception en UpdateOperator: {ex.GetType().Name} - {ex.Message}");
                throw new InvalidOperationException("Error al actualizar el operador. Intente nuevamente.");
            }
        }
        public Operator? ValidateOper(AuthenticationDTO authDTO)
        {
            // Usa el método heredado del repositorio para validar el usuario.
            return _operatorRepository.GetUserByNLegajoAndPassword(authDTO.NLegajo, authDTO.Password);
        }
    }
}

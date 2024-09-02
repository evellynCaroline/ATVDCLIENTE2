using ClienteAtv.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class ClienteServico
{
    private readonly string _filePath = "clientes.json";

    public ClienteServico()
    {
        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Dispose();
            File.WriteAllText(_filePath, "[]");
        }
    }

    private List<Cliente> GetClientes()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();
    }

    private void SaveClientes(List<Cliente> clientes)
    {
        var json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void AddCliente(Cliente cliente)
    {
        if (string.IsNullOrEmpty(cliente.Nome) ||
            cliente.DataNascimento == default ||
            string.IsNullOrEmpty(cliente.Sexo) ||
            string.IsNullOrEmpty(cliente.RG) ||
            string.IsNullOrEmpty(cliente.CPF) ||
            string.IsNullOrEmpty(cliente.Endereco) ||
            string.IsNullOrEmpty(cliente.Cidade) ||
            string.IsNullOrEmpty(cliente.Estado) ||
            string.IsNullOrEmpty(cliente.Telefone) ||
            string.IsNullOrEmpty(cliente.Email))
        {
            throw new ArgumentException("Todos os campos são obrigatórios.");
        }

        if (!ValidarCPF(cliente.CPF))
        {
            throw new ArgumentException("CPF inválido.");
        }

        var clientes = GetClientes();
        clientes.Add(cliente);
        SaveClientes(clientes);
    }

    public List<Cliente> GetAllClientes()
    {
        return GetClientes();
    }

    public Cliente GetClienteByCPF(string cpf)
    {
        return GetClientes().FirstOrDefault(c => c.CPF == cpf);
    }

    public void UpdateCliente(string cpf, Cliente updatedCliente)
    {
        if (string.IsNullOrEmpty(updatedCliente.Nome) ||
            updatedCliente.DataNascimento == default ||
            string.IsNullOrEmpty(updatedCliente.Sexo) ||
            string.IsNullOrEmpty(updatedCliente.RG) ||
            string.IsNullOrEmpty(updatedCliente.CPF) ||
            string.IsNullOrEmpty(updatedCliente.Endereco) ||
            string.IsNullOrEmpty(updatedCliente.Cidade) ||
            string.IsNullOrEmpty(updatedCliente.Estado) ||
            string.IsNullOrEmpty(updatedCliente.Telefone) ||
            string.IsNullOrEmpty(updatedCliente.Email))
        {
            throw new ArgumentException("Todos os campos são obrigatórios.");
        }

        if (!ValidarCPF(updatedCliente.CPF))
        {
            throw new ArgumentException("CPF inválido.");
        }

        var clientes = GetClientes();
        var index = clientes.FindIndex(c => c.CPF == cpf);

        if (index == -1)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        clientes[index] = updatedCliente;
        SaveClientes(clientes);
    }

    public void DeleteCliente(string cpf)
    {
        var clientes = GetClientes();
        var clienteToRemove = clientes.FirstOrDefault(c => c.CPF == cpf);

        if (clienteToRemove == null)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        clientes.Remove(clienteToRemove);
        SaveClientes(clientes);
    }

    private static bool ValidarCPF(string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
        {
            return false;
        }

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        int soma1 = 0;
        for (int i = 0; i < 9; i++)
        {
            soma1 += (cpf[i] - '0') * multiplicadores1[i];
        }

        int digitoVerificador1 = (soma1 % 11 < 2) ? 0 : 11 - (soma1 % 11);

        int soma2 = 0;
        for (int i = 0; i < 10; i++)
        {
            soma2 += (cpf[i] - '0') * multiplicadores2[i];
        }

        int digitoVerificador2 = (soma2 % 11 < 2) ? 0 : 11 - (soma2 % 11);

        return cpf[9] == digitoVerificador1.ToString()[0] && cpf[10] == digitoVerificador2.ToString()[0];
    }
}

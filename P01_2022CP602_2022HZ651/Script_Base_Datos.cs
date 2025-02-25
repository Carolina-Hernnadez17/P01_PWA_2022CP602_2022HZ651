using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;

namespace P01_2022CP602_2022HZ651
{
    public class Script_Base_Datos
    {
    //    --Base de datos Parqueo--
    //    CREATE DATABASE ParqueoDB;

    //    USE ParqueoDB;


    //        CREATE TABLE Usuarios(
    //            Id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    //            Nombre VARCHAR(100) NOT NULL,
    //            Correo VARCHAR(100) NOT NULL,
    //        Telefono VARCHAR(15) NOT NULL,
    //            Contrasena VARCHAR(255) NOT NULL,
    //            Rol VARCHAR(50) NOT NULL 
    //);

    //        CREATE TABLE Sucursales(
    //            Id_sucursal INT IDENTITY(1,1) PRIMARY KEY,
    //            Nombre VARCHAR(100) NOT NULL,
    //            Direccion VARCHAR(255) NOT NULL,
    //        Telefono VARCHAR(15) NOT NULL,
    //            Id_usuario INT NOT NULL,
    //    EspaciosDisponibles INT NOT NULL
    //);

    //        CREATE TABLE EspaciosParqueo(
    //            Id_espacioparqueo INT IDENTITY(1,1) PRIMARY KEY,
    //            Numero INT NOT NULL,
    //    Ubicacion VARCHAR(100) NOT NULL,
    //    CostoPorHora DECIMAL(10,2) NOT NULL,
    //    Estado VARCHAR(20) NOT NULL,
    //    Id_sucursal INT NOT NULL
    //);

    //CREATE TABLE Reservas(
    //    Id_reservas INT IDENTITY(1,1) PRIMARY KEY,
    //    Id_usuario INT NOT NULL,
    //    Id_espacioparqueo INT NOT NULL,
    //    Fecha DATE NOT NULL,
    //    HoraInicio TIME NOT NULL,
    //    CantidadHoras INT NOT NULL,
    //    Estado VARCHAR(20) NOT NULL,
    //);
    }
}

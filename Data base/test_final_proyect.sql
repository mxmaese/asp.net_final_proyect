-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 20-03-2024 a las 22:24:38
-- Versión del servidor: 10.4.22-MariaDB
-- Versión de PHP: 7.4.27

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `test_final_proyect`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `address_users`
--

CREATE TABLE `address_users` (
  `Id_Address` int(11) NOT NULL,
  `Id_User` int(11) NOT NULL,
  `Line_1` varchar(150) NOT NULL,
  `Line_2` varchar(150) DEFAULT NULL,
  `Zip_Code` varchar(20) NOT NULL,
  `City` varchar(150) NOT NULL,
  `State` varchar(150) DEFAULT NULL,
  `Country` varchar(100) DEFAULT NULL,
  `Message` varchar(200) DEFAULT NULL,
  `Verificated` bit(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `profiles`
--

CREATE TABLE `profiles` (
  `Id_Profiles` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Type_User` int(11) NOT NULL,
  `Description` varchar(400) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `profiles`
--

INSERT INTO `profiles` (`Id_Profiles`, `Name`, `Type_User`, `Description`) VALUES
(1, 'Visitante', 1, 'No compro todavia'),
(2, 'Comprador', 1, 'Ya ha comprado'),
(3, 'Administrador', 0, 'La persona indicada de administrar'),
(4, 'Vendedor', 0, 'el que vende');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

CREATE TABLE `users` (
  `Id_User` int(11) NOT NULL,
  `Login` varchar(25) NOT NULL,
  `Name` varchar(250) NOT NULL,
  `User_Types` int(11) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Validated_Email` bit(11) NOT NULL,
  `Active` bit(11) NOT NULL,
  `Register_Date` date NOT NULL,
  `Birthday_Date` date NOT NULL,
  `Hashed_Password` varchar(100) DEFAULT NULL,
  `Image` varchar(100) DEFAULT NULL,
  `Last_Login` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users_profiles`
--

CREATE TABLE `users_profiles` (
  `User_Id` int(11) NOT NULL,
  `Profiles_Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `address_users`
--
ALTER TABLE `address_users`
  ADD PRIMARY KEY (`Id_Address`),
  ADD UNIQUE KEY `Id_Address` (`Id_Address`);

--
-- Indices de la tabla `profiles`
--
ALTER TABLE `profiles`
  ADD PRIMARY KEY (`Id_Profiles`),
  ADD UNIQUE KEY `Id_Profiles` (`Id_Profiles`,`Name`);

--
-- Indices de la tabla `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id_User`),
  ADD UNIQUE KEY `Id_User` (`Id_User`),
  ADD UNIQUE KEY `Login` (`Login`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `Id_User_2` (`Id_User`),
  ADD KEY `Id_User_3` (`Id_User`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `address_users`
--
ALTER TABLE `address_users`
  MODIFY `Id_Address` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=0;

--
-- AUTO_INCREMENT de la tabla `profiles`
--
ALTER TABLE `profiles`
  MODIFY `Id_Profiles` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `Id_User` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=0;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

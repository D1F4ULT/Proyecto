CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Quantity INT NOT NULL
);

INSERT INTO Products (Name, Description, Price, Quantity)
VALUES 
('Proyector mini CHOWA Proyectores B2 300lm blanco 110V', 'Mini proyector posee las mismas capacidades que uno tradicional con la ventaja de ser portable', 464.264, 50),
('Silla Escritorio Ergonomica Sillon Oficina Giratoria Ejecutiva Secretarial Pc Computador Femmto', 'Silla Femmto ergonomica y confiable', 497.900, 30),
('Power Bank Mini Port�til', 'Power Bank Mini Port�til Bater�a 5000mah Con Built In 3 Cables 1hora Usb C Baterias Para Celular Compatible Con iPhone Samsung Android', 38.336, 70),
('Caixun C32VJHZ Televisor 32 Hd Smart Led Android', ' Smart TV C32VJHZ resoluci�n HD te presentar� im�genes con gran detalle y alta definici�n. Ahora todo lo que veas cobrar� vida con brillo y colores m�s reales.', 550.900, 40);
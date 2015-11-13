SET NOCOUNT ON;

DECLARE @COURSE_course_state INT;
SELECT @COURSE_course_state = PK_State_Id FROM States WHERE Type = 'COURSE' AND Name = 'Activo';

SET IDENTITY_INSERT Courses ON;

INSERT INTO Courses (PK_Course_Id, Name, Code, FK_State_Id) VALUES
(1, 'parturient montes nascetur ridiculus mus', '282-79-7322', @COURSE_course_state),
(2, 'arcu libero rutrum ac lobortis vel dapibus at diam nam', '972-23-9347', @COURSE_course_state),
(3, 'ac consequat metus sapien ut nunc vestibulum ante ipsum primis', '978-19-5577', @COURSE_course_state),
(4, 'cras non velit nec nisi', '293-90-9536', @COURSE_course_state),
(5, 'in purus eu magna vulputate luctus cum sociis', '717-87-6336', @COURSE_course_state),
(6, 'eget orci vehicula condimentum curabitur', '550-75-3280', @COURSE_course_state),
(7, 'neque duis bibendum morbi non quam nec dui', '156-34-2168', @COURSE_course_state),
(8, 'aliquam augue quam sollicitudin vitae consectetuer', '713-81-9816', @COURSE_course_state),
(9, 'integer non velit donec diam neque', '705-31-6473', @COURSE_course_state),
(10, 'vestibulum sit amet cursus id', '904-57-6333', @COURSE_course_state),
(11, 'id nulla ultrices aliquet maecenas leo odio condimentum id', '865-42-8497', @COURSE_course_state),
(12, 'nisi volutpat eleifend donec ut dolor morbi', '435-89-8630', @COURSE_course_state),
(13, 'nunc proin at turpis a pede posuere', '147-87-9941', @COURSE_course_state),
(14, 'neque libero convallis eget eleifend luctus', '187-22-5866', @COURSE_course_state),
(15, 'ut massa volutpat convallis morbi odio odio elementum eu interdum', '278-17-3540', @COURSE_course_state),
(16, 'sapien dignissim vestibulum vestibulum ante ipsum', '316-73-4717', @COURSE_course_state),
(17, 'etiam faucibus cursus urna ut tellus nulla', '233-26-0973', @COURSE_course_state),
(18, 'mauris enim leo rhoncus sed vestibulum sit amet cursus', '123-99-7300', @COURSE_course_state),
(19, 'id consequat in consequat ut nulla sed accumsan', '363-23-8462', @COURSE_course_state),
(20, 'purus aliquet at feugiat non pretium quis', '142-98-2009', @COURSE_course_state),
(21, 'nisl venenatis lacinia aenean sit amet justo', '165-22-1090', @COURSE_course_state),
(22, 'feugiat et eros vestibulum ac est lacinia nisi venenatis', '197-78-1188', @COURSE_course_state),
(23, 'et ultrices posuere cubilia curae', '768-58-6699', @COURSE_course_state),
(24, 'orci eget orci vehicula condimentum curabitur', '624-70-4388', @COURSE_course_state),
(25, 'in sagittis dui vel nisl duis', '293-29-0634', @COURSE_course_state),
(26, 'est quam pharetra magna ac consequat metus sapien ut', '482-79-0717', @COURSE_course_state),
(27, 'in lectus pellentesque at nulla suspendisse potenti cras', '530-88-8880', @COURSE_course_state),
(28, 'vestibulum ante ipsum primis in', '918-17-5600', @COURSE_course_state),
(29, 'augue vestibulum rutrum rutrum neque aenean auctor gravida', '527-17-8538', @COURSE_course_state),
(30, 'nisi eu orci mauris lacinia sapien quis', '536-36-6197', @COURSE_course_state),
(31, 'urna pretium nisl ut volutpat sapien arcu sed augue aliquam', '543-51-5891', @COURSE_course_state),
(32, 'potenti cras in purus eu magna vulputate', '538-50-9127', @COURSE_course_state),
(33, 'turpis sed ante vivamus tortor duis mattis egestas metus', '415-91-7860', @COURSE_course_state),
(34, 'placerat praesent blandit nam nulla integer', '440-97-0099', @COURSE_course_state),
(35, 'vel pede morbi porttitor lorem id ligula suspendisse', '244-23-7250', @COURSE_course_state),
(36, 'pharetra magna ac consequat metus sapien ut nunc', '958-48-0997', @COURSE_course_state),
(37, 'sed accumsan felis ut at dolor quis odio', '523-89-1622', @COURSE_course_state),
(38, 'eget orci vehicula condimentum curabitur in', '488-56-8799', @COURSE_course_state),
(39, 'luctus et ultrices posuere cubilia', '725-66-1810', @COURSE_course_state),
(40, 'ac nibh fusce lacus purus aliquet', '641-76-5351', @COURSE_course_state),
(41, 'at nulla suspendisse potenti cras in', '560-71-4202', @COURSE_course_state),
(42, 'donec semper sapien a libero nam dui', '648-60-6904', @COURSE_course_state),
(43, 'in sagittis dui vel nisl', '930-04-6752', @COURSE_course_state),
(44, 'ut blandit non interdum in ante vestibulum ante', '580-33-8392', @COURSE_course_state),
(45, 'magna vestibulum aliquet ultrices erat', '626-87-4475', @COURSE_course_state),
(46, 'consectetuer eget rutrum at lorem integer tincidunt', '819-34-2586', @COURSE_course_state),
(47, 'nulla tempus vivamus in felis eu', '728-47-9618', @COURSE_course_state),
(48, 'odio condimentum id luctus nec', '814-19-6720', @COURSE_course_state),
(49, 'commodo placerat praesent blandit nam nulla integer', '366-98-3117', @COURSE_course_state),
(50, 'placerat praesent blandit nam nulla integer pede justo', '575-59-4449', @COURSE_course_state),
(51, 'morbi ut odio cras mi pede malesuada in imperdiet', '308-35-2672', @COURSE_course_state),
(52, 'in tempus sit amet sem fusce', '112-70-4060', @COURSE_course_state),
(53, 'blandit mi in porttitor pede justo eu massa donec', '840-10-0170', @COURSE_course_state),
(54, 'augue vestibulum ante ipsum primis in faucibus orci luctus et', '359-46-3655', @COURSE_course_state),
(55, 'nec sem duis aliquam convallis nunc proin', '868-56-9293', @COURSE_course_state),
(56, 'purus sit amet nulla quisque arcu libero rutrum', '214-50-3885', @COURSE_course_state),
(57, 'orci eget orci vehicula condimentum curabitur', '440-61-5091', @COURSE_course_state),
(58, 'est risus auctor sed tristique', '407-88-9447', @COURSE_course_state),
(59, 'massa quis augue luctus tincidunt nulla mollis molestie lorem quisque', '422-67-4063', @COURSE_course_state),
(60, 'dapibus nulla suscipit ligula in lacus curabitur at ipsum ac', '578-40-7558', @COURSE_course_state),
(61, 'aliquam erat volutpat in congue etiam justo etiam pretium iaculis', '686-57-1293', @COURSE_course_state),
(62, 'semper est quam pharetra magna ac consequat metus sapien ut', '471-67-8438', @COURSE_course_state),
(63, 'ut at dolor quis odio consequat varius integer', '590-96-1522', @COURSE_course_state),
(64, 'maecenas leo odio condimentum id', '719-67-7416', @COURSE_course_state),
(65, 'dolor morbi vel lectus in quam fringilla', '104-40-6137', @COURSE_course_state),
(66, 'nulla sed accumsan felis ut at dolor quis odio consequat', '692-06-4993', @COURSE_course_state),
(67, 'erat nulla tempus vivamus in felis eu', '768-28-3967', @COURSE_course_state),
(68, 'nulla nisl nunc nisl duis', '790-04-0598', @COURSE_course_state),
(69, 'tortor risus dapibus augue vel accumsan', '870-68-0881', @COURSE_course_state),
(70, 'etiam pretium iaculis justo in', '419-22-6831', @COURSE_course_state),
(71, 'nisi vulputate nonummy maecenas tincidunt lacus', '684-66-9249', @COURSE_course_state),
(72, 'natoque penatibus et magnis dis parturient montes', '761-37-2948', @COURSE_course_state),
(73, 'mauris non ligula pellentesque ultrices phasellus id sapien in', '195-36-6678', @COURSE_course_state),
(74, 'pede malesuada in imperdiet et commodo vulputate justo', '477-79-0571', @COURSE_course_state),
(75, 'consectetuer eget rutrum at lorem integer tincidunt ante vel', '334-90-9281', @COURSE_course_state),
(76, 'rhoncus aliquet pulvinar sed nisl nunc rhoncus dui vel sem', '619-53-9937', @COURSE_course_state),
(77, 'lacinia aenean sit amet justo morbi ut', '340-90-4590', @COURSE_course_state),
(78, 'tincidunt eu felis fusce posuere felis sed lacus morbi', '953-25-0561', @COURSE_course_state),
(79, 'nisl duis bibendum felis sed interdum venenatis turpis', '869-42-1357', @COURSE_course_state),
(80, 'faucibus accumsan odio curabitur convallis duis consequat', '999-54-7145', @COURSE_course_state),
(81, 'nisl nunc nisl duis bibendum felis', '718-22-4640', @COURSE_course_state),
(82, 'quis orci nullam molestie nibh in lectus pellentesque at nulla', '645-40-8117', @COURSE_course_state),
(83, 'integer ac leo pellentesque ultrices mattis odio donec vitae nisi', '140-04-0007', @COURSE_course_state),
(84, 'erat quisque erat eros viverra eget', '534-94-3811', @COURSE_course_state),
(85, 'leo odio porttitor id consequat', '909-17-9933', @COURSE_course_state),
(86, 'tincidunt lacus at velit vivamus vel', '411-31-2580', @COURSE_course_state),
(87, 'quisque arcu libero rutrum ac lobortis vel', '539-08-9402', @COURSE_course_state),
(88, 'consectetuer eget rutrum at lorem', '288-16-2300', @COURSE_course_state),
(89, 'penatibus et magnis dis parturient montes nascetur ridiculus mus etiam', '675-51-9614', @COURSE_course_state),
(90, 'nulla pede ullamcorper augue a suscipit nulla elit ac', '394-56-8449', @COURSE_course_state),
(91, 'justo pellentesque viverra pede ac diam cras pellentesque volutpat', '946-03-4450', @COURSE_course_state),
(92, 'laoreet ut rhoncus aliquet pulvinar sed nisl nunc rhoncus dui', '757-35-2067', @COURSE_course_state),
(93, 'interdum mauris ullamcorper purus sit amet nulla quisque arcu', '118-39-9185', @COURSE_course_state),
(94, 'potenti in eleifend quam a odio in hac habitasse', '544-40-5250', @COURSE_course_state),
(95, 'praesent lectus vestibulum quam sapien varius', '970-28-5391', @COURSE_course_state),
(96, 'porttitor lacus at turpis donec posuere metus vitae ipsum aliquam', '244-20-7472', @COURSE_course_state),
(97, 'eget congue eget semper rutrum nulla nunc', '733-40-2106', @COURSE_course_state),
(98, 'nulla elit ac nulla sed vel enim', '213-95-8140', @COURSE_course_state),
(99, 'potenti nullam porttitor lacus at', '997-03-4174', @COURSE_course_state),
(100, 'pretium iaculis diam erat fermentum justo nec condimentum', '712-17-6660', @COURSE_course_state);

SET IDENTITY_INSERT Courses OFF;
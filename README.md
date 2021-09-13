# webapp
Курс "ASP.NET Core Веб Приложение"
Урок 2 Приложение Timesheets
Задание обсуждалось на уроках, некоторые моменты могут отличаться от того, как было задумано. Старался учесть все, 
что объяснялось преподавателем и было в чате Телеграма.

Модель приложения и описание сущностей:
Заказчик (customer) заключает договор на оказание работ (contract). У сущности заказчика хранятся ссылки на заключенные контракты и 
банковский счет. 
Контракт (contract) содержит списки задач (task), счетов на задачи (invoice) и ссылку на заказчика.
Задание (task) содержит список ссылок на промежуточные сущности taskEmployee (информация, которая касается работника, выполняющего
это задание) и ссылку на свой контракт.
Сущность taskEmployee содержит ссылку на задачу, работника, который выполняет это задание, статус задания (выполнено или нет) и количество
затраченных на задание часов.
Счет (invoice) создается автоматически при выполнении задания (task), содержит ссылки на контракт, задание, стоимость выполненных работ и 
статус оплаты (оплачено или нет).
Работник (employee) прикрепляется к заданию (task), содержит список ссылок на свои задания, свой банковский счет и ставку, сколько стоит
его работа в час.

Сущности:
- CustomerDto:
  int Id,
  double BankAccount,
  List<ContractDto> Contracts
  
  - ContractDto:
    int Id,
    CustomerDto Customer,
    List<TaskDto> Tasks,
    List<InvoiceDto> Invoices
    
  - TaskDto:
    int Id,
    ContractDto Contract,
    List<TaskEmployeesDto> Employees
    
  - TaskEmployeeDto:
    int Id,
    TaskDto Task,
    EmployeeDto Employee,
    bool Done,
    double Hours
    
  - InvoiceDto:
    int Id,
    ContractDto Contract,
    TaskDto Task,
    double Cost,
    bool PayDone
    
  - EmployeeDto:
    int Id,
    double Rate,
    double BankAccount,
    List<TaskDto> Tasks.
    
Контроллеры:
  - CustomerController:
    api/customer
    HttpPost RegisterCustomer() //регистрация заказчика, действие по умолчанию
    
    api/customer/id 
    HttpGet GetCustomer([FromRoute] int id) //получить заказчика по id
  
    api/customer/all
    HttpGet GetCustomerAll() //получить всех заказчиков
    
    api/customer/id/contract
    HttpPost RegisterContract([FromRoute] int id) //зарегистрировать контракт по customerId
    
    api/customer/id/contract/contractid
    HttpGet GetContract([FromRoute] int id, [FromRoute] int contractId) //получить контракт заказчика по id
    
    api/customer/id/contract/all
    HttpGet GetContractAll([FromRoute] int id) //получить все контракты заказчика
    
    api/customer/id/contract/contractid/task/taskid/invoice
    HttpGet GetInvoice([FromRoute] int id, [FromRoute] int contractId, [FromRoute] int taskId) //получить счет по задаче контракта
    
    api/customer/id/contract/contractid/invoice/all
    HttpGet GetInvoiceAll([FromRoute] int id, [FromRoute] int contractId) //получить все счета по контракту
    
    api/customer/id/contract/contractid/task
    HttpPost RegisterTask([FromRoute] int id, [FromRoute] int contractId) //зарегистрировать задачу
    
    api/customer/id/contract/contractid/task/taskid
    HttpGet GetTask([FromRoute] int id, [FromRoute] int contractId, [FromRoute] int taskId) //получить задачу по id
    
    api/customer/id/contract/contractid/task/all
    HttpGet GetTaskAll([FromRoute] int id, [FromRoute] int contractId) //получить все задачи контракта
    
  - EmployeeController:
    api/employee/rate/rateid
    HttpPost RegisterEmployee([FromRoute] double rate) //зарегистрировать работника и указать его ставку
    
    api/employee/id
    HttpGet GetEmployee([FromRoute] int id) //получить работника по id
    
    api/employee/all
    HttpGet GetEmployeeAll() //получить всех работников
    
    api/employee/id/customer/customerid/contract/contractid/task/taskid
    HttpPost RegisterTaskToEmployee([FromRoute] int id, [FromRoute] int customerId, [FromRoute] int contractId, [FromRoute] int taskId)
    //назначить задачу работнику
    
    api/employee/id/task/all
    HttpGet GetEmployeeTasks([FromRoute] int id) //посмотреть задачи работника
    
    api/employee/id/customer/customerid/contract/contractid/task/taskid/hours/nhours/done
    HttpPost SetTaskIsDone([FromRoute] int id, [FromRoute] int customerId, [FromRoute] int contractId, [FromRoute] int taskId, 
    [FromRoute] double nhours) //выполнить задание
    
    api/employee/id/customer/customerid/contractid/task/taskid/status
    HttpGet GetTaskStatus([FromRoute] int id, [FromRoute] int customerId, [FromRoute] int contractId, [FromRoute] int taskId)
    //получить статус задания
    
    api/employee/id/bankaccount
    HttpGet GetEmployeeBankAccount([FromRoute] int id) //посмотреть счет работника
   
Информация хранится в списках customers, contracts, tasks, invoices, employees класса DictionariesGlobal. Для простоты в этом классе
существуют счетчики id, которые увеличиваются при создании новых экземпляров сущностей.

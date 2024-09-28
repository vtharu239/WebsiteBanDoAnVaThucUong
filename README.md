1) Copy web URL của project
![image](https://github.com/user-attachments/assets/0bcbbdc7-e6eb-41be-9f82-2872cba981c7)

2) Mở Visual Studio, trên thanh menu, Chọn Git > Clone Repository
![image](https://github.com/user-attachments/assets/57b9d6a8-9f20-4669-b464-d4ef802a59d1)

3) Dán link URL vào, chọn location bạn muốn lưu loacal project. Sau đó nhấn Clone
![image](https://github.com/user-attachments/assets/e768b6d3-f6d6-4e4f-87fa-d28d0ebafde7)

4) Vào file web.config, ở đường dẫn connectionString, hãy thay đổi thành Data Source của bạn (là Server Name trong SQL Server)

// Copy connectionString bên dưới và đổi Data Source nhe
<add name="DefaultConnection" providerName="System.Data.SqlClient" 
     connectionString="Data Source=DESKTOP-7L0TGLV\SQLEXPRESS;Initial Catalog=WebsiteBanDoAnVaThucUong;
     Integrated Security=True;MultipleActiveResultSets=True" />
     
![image](https://github.com/user-attachments/assets/6e34d0f5-ed14-4159-96d3-762780b58477)

6) Sau đó chạy file script để lấy được dữ liệu

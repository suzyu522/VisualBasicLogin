Imports System.Data.SqlClient
Public Class Form1
    'Create database connection
    Dim connection As New SqlConnection("Server = DESKTOP-MUR6H03; Database = Login; Integrated Security = true")

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim userName As String = TextBox1.Text
        Dim passWord As String = TextBox2.Text
        Dim command As New SqlCommand("select * from Table_User where UserName = @username and Password = @password", connection)
        command.Parameters.Add("@username", SqlDbType.VarChar).Value = userName
        command.Parameters.Add("@password", SqlDbType.VarChar).Value = passWord
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        If table.Rows.Count <= 0 Then
            MessageBox.Show("Username or Password are invalid.")
        Else
            'MessageBox.Show("Login Successful")
            Form2.SetUserName = userName
            Dim userId As String = ""
            Using cmdObj As New SqlClient.SqlCommand("select Id from Table_User where UserName = '" & userName & "' and password = '" & passWord & "'", connection)
                connection.Open()
                Using readerObj As SqlClient.SqlDataReader = cmdObj.ExecuteReader
                    While readerObj.Read
                        userId = readerObj("Id").ToString
                    End While
                End Using
                connection.Close()
            End Using
            Form2.setLabel(userId)
            Form2.Show()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim command As New SqlCommand("insert into Table_User(UserName,Password) values('" & TextBox1.Text & "','" & TextBox2.Text & "')", connection)
        connection.Open()
        If command.ExecuteNonQuery() = 1 Then
            MessageBox.Show("New User Added")
        Else
            MessageBox.Show("User Not added")
        End If
        connection.Close()
    End Sub


End Class

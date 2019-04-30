Imports System.Data.SqlClient

Public Class Form3
    Dim connection As New SqlConnection("Server = DESKTOP-MUR6H03; Database = Login; Integrated Security = true")
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillTable()
    End Sub
    Private Sub FillTable()
        Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price FROM Book", connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        DataGridView1.DataSource = table
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim index As Integer
        index = e.RowIndex
        Dim selectedRow As DataGridViewRow
        selectedRow = DataGridView1.Rows(index)
        TextBox1.Text = selectedRow.Cells(0).Value.ToString()
        TextBox2.Text = selectedRow.Cells(1).Value.ToString()
        TextBox3.Text = selectedRow.Cells(2).Value.ToString()
        TextBox4.Text = selectedRow.Cells(3).Value.ToString()
        TextBox5.Text = selectedRow.Cells(4).Value.ToString()
    End Sub


    Private Sub btnOrder_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then
            MessageBox.Show("Please complete the form.")
        Else
            Dim command As New SqlCommand("select * from Book where Title = @title and Author = @author", connection)
            command.Parameters.Add("@title", SqlDbType.VarChar).Value = TextBox1.Text
            command.Parameters.Add("@author", SqlDbType.VarChar).Value = TextBox2.Text
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
            If table.Rows.Count <= 0 Then
                MessageBox.Show("The book does not exist in the database.")
            Else
                Dim bookId As String = ""
                Using cmdObj As New SqlClient.SqlCommand("select Id from Book where Title = '" & TextBox1.Text & "' and author = '" & TextBox2.Text & "'", connection)
                    connection.Open()
                    Using readerObj As SqlDataReader = cmdObj.ExecuteReader
                        While readerObj.Read
                            bookId = readerObj("Id").ToString
                        End While
                    End Using
                    connection.Close()
                End Using
                Dim userId = Form2.getId
                ' Check if record exists
                Dim command1 As New SqlCommand("SELECT * FROM Purchase WHERE UserId = @userId and BookId = @bookId", connection)
                command1.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId
                command1.Parameters.Add("@bookId", SqlDbType.VarChar).Value = bookId
                Dim adapter1 As New SqlDataAdapter(command1)
                Dim table1 As New DataTable()
                adapter1.Fill(table1)
                Dim quantity = "0"
                If table1.Rows.Count <= 0 Then ' Record does not exist
                    quantity = "1"
                    Dim command2 As New SqlCommand("insert into Purchase(UserID,BookId,quantity) values('" & userId & "','" & bookId & "' ,'" & quantity & "')", connection)
                    connection.Open()
                    If command2.ExecuteNonQuery() = 1 Then
                        MessageBox.Show("Book ordered successfully.")
                    Else
                        MessageBox.Show("Book order unsuccessful.")
                    End If
                    connection.Close()
                Else ' Increment the quantity value
                    Dim command3 As New SqlCommand("update Purchase set quantity = quantity + 1 where UserId = @userId and BookId = @bookId", connection)
                    command3.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId
                    command3.Parameters.Add("@bookId", SqlDbType.VarChar).Value = bookId
                    connection.Open()
                    If command3.ExecuteNonQuery() = 1 Then
                        MessageBox.Show("Book ordered successfully.")
                    Else
                        MessageBox.Show("Book order unsuccessful.")
                    End If
                    connection.Close()
                End If
            End If
        End If

        Form2.FillTable()
    End Sub
End Class
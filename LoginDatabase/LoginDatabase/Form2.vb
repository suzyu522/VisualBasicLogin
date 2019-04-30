Imports System.Data.SqlClient

Public Class Form2
    Dim userName As String
    Dim userId As String
    Dim comboBoxValue As String = ""
    Dim bookTitle As String
    Dim author As String
    Dim connection As New SqlConnection("Server = DESKTOP-MUR6H03; Database = Login; Integrated Security = true")
    Public WriteOnly Property SetUserName As String
        Set(value As String)
            userName = value
        End Set
    End Property
    Public Sub setLabel(id As String)
        lblUserName.Text = "Welcome back " + userName + "."
        userId = id
    End Sub
    Public Function getId() As String
        Return userId
    End Function
    Private Sub BtnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Form2.ActiveForm.Close()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillTable()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FillTable()
    End Sub


    Public Sub FillTable()
        Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price, Purchase.quantity FROM Book INNER JOIN Purchase ON Book.Id = Purchase.BookId WHERE UserId = @ID", connection)
        command.Parameters.Add("@ID", SqlDbType.VarChar).Value = userId
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        DataGridView1.DataSource = table
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim searchText = TextBox1.Text
        If Me.ComboBox1.SelectedIndex = 0 Then
            comboBoxValue = ComboBox1.SelectedText
            Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price, Purchase.quantity FROM Book INNER JOIN Purchase ON Book.Id = Purchase.BookId WHERE Title = @title", connection)
            command.Parameters.Add("@title", SqlDbType.VarChar).Value = searchText
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
            DataGridView1.DataSource = table
        ElseIf Me.ComboBox1.SelectedIndex = 1 Then
            comboBoxValue = ComboBox1.SelectedText
            Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price, Purchase.quantity FROM Book INNER JOIN Purchase ON Book.Id = Purchase.BookId WHERE Author = @author", connection)
            command.Parameters.Add("@author", SqlDbType.VarChar).Value = searchText
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
            DataGridView1.DataSource = table
        ElseIf Me.ComboBox1.SelectedIndex = 2 Then 'genre
            comboBoxValue = ComboBox1.SelectedText
            Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price, Purchase.quantity FROM Book INNER JOIN Purchase ON Book.Id = Purchase.BookId WHERE Genre = @genre", connection)
            command.Parameters.Add("@genre", SqlDbType.VarChar).Value = searchText
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
        ElseIf Me.ComboBox1.SelectedIndex = 3 Then 'publication
            comboBoxValue = ComboBox1.SelectedText
            Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price, Purchase.quantity FROM Book INNER JOIN Purchase ON Book.Id = Purchase.BookId WHERE PublicationDate = @publication", connection)
            command.Parameters.Add("@publication", SqlDbType.VarChar).Value = searchText
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
        ElseIf Me.ComboBox1.SelectedIndex = 4 Then 'price
            comboBoxValue = ComboBox1.SelectedText
            Dim command As New SqlCommand("SELECT Title, Author, Genre, PublicationDate, Price, Purchase.quantity FROM Book INNER JOIN Purchase ON Book.Id = Purchase.BookId WHERE Price = @price", connection)
            command.Parameters.Add("@price", SqlDbType.VarChar).Value = searchText
            Dim adapter As New SqlDataAdapter(command)
            Dim table As New DataTable()
            adapter.Fill(table)
        Else
            ' FillTable()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form3.Show()

    End Sub

    ' Remove Record
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If bookTitle = "" And author = "" Then
            MessageBox.Show("Select a book to delete.")
        Else
            'Check Quantity
            Dim quantity = GetQuantity()
            Dim bookId = GetBookId()
            If quantity <= 1 Then
                Dim command As New SqlCommand("delete from purchase WHERE UserId = @userId and BookId = @BookId", connection)
                command.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId
                command.Parameters.Add("@BookId", SqlDbType.VarChar).Value = bookId
                Dim adapter As New SqlDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)
            Else
                Dim command3 As New SqlCommand("update Purchase set quantity = quantity - 1 where UserId = @userId and BookId = @bookId", connection)
                command3.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId
                command3.Parameters.Add("@bookId", SqlDbType.VarChar).Value = bookId
                connection.Open()
                If command3.ExecuteNonQuery() = 1 Then
                    MessageBox.Show("Book order cancelled successfully.")
                Else
                    MessageBox.Show("Book cancel unsuccessful.")
                End If
                connection.Close()

            End If
        End If


        FillTable()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim index As Integer
        index = e.RowIndex
        Dim selectedRow As DataGridViewRow
        selectedRow = DataGridView1.Rows(index)
        bookTitle = selectedRow.Cells(0).Value.ToString()
        author = selectedRow.Cells(1).Value.ToString()
    End Sub

    Private Function GetBookId()
        Dim bookId As String = ""
        Using cmdObj As New SqlClient.SqlCommand("select Id from Book where Title = '" & bookTitle & "' and author = '" & author & "'", connection)
            connection.Open()
            Using readerObj As SqlDataReader = cmdObj.ExecuteReader
                While readerObj.Read
                    bookId = readerObj("Id").ToString
                End While
            End Using
            connection.Close()
        End Using
        Return bookId
    End Function

    Private Function GetQuantity()
        Dim quantity As Integer = 0
        Dim bookId = GetBookId()
        Using cmdObj As New SqlClient.SqlCommand("select quantity from Purchase where UserId = '" & userId & "' and BookId = '" & bookId & "'", connection)
            connection.Open()
            Using readerObj As SqlDataReader = cmdObj.ExecuteReader
                While readerObj.Read
                    quantity = readerObj("quantity").ToString
                End While
            End Using
            connection.Close()
        End Using
        Return quantity

    End Function

End Class
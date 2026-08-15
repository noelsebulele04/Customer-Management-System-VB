Imports System.IO
Public Class frmCustomerManagementSystem

    'Structure to hold one customer record
    Structure Customer
        Dim CustomerID As Integer
        Dim Name As String
        Dim Email As String
        Dim Phone As String
        Dim Address As String
    End Structure

    'Array to store up to 15 customers (IDs 0-14)
    Dim Customers(14) As Customer

    'Counter to track how many records are loaded
    Dim RecordCount As Integer = 0

    Private Sub btnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click
        Try
            'Open the text file
            Dim sr As New StreamReader("customers.txt")
            Dim line As String
            RecordCount = 0

            'Read each line until the end of the file
            Do While Not sr.EndOfStream
                line = sr.ReadLine()
                Dim fields() As String = line.Split(","c)

                'Store each field into the customers array
                Customers(RecordCount).CustomerID = CInt(fields(0))
                Customers(RecordCount).Name = fields(1)
                Customers(RecordCount).Email = fields(2)
                Customers(RecordCount).Phone = fields(3)
                Customers(RecordCount).Address = fields(4)

                RecordCount += 1
            Loop

            sr.Close()
            'Confirmation Message
            MessageBox.Show("Data has been succesfully read and ready to display.")

        Catch ex As Exception
            MessageBox.Show("Error loading file: " & ex.Message)
        End Try
    End Sub


    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        'Clear the listbox first
        lstReceipt.Items.Clear()

        'Loop through all loaded records
        For i As Integer = 0 To RecordCount - 1
            'Show only Customer info in the listbox
            lstReceipt.Items.Add($"{Customers(i).CustomerID} - {Customers(i).Name} - {Customers(i).Email} - {Customers(i).Phone} - {Customers(i).Address}")
        Next
    End Sub

    Private Sub lstReciept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstReceipt.SelectedIndexChanged

        If lstReceipt.SelectedIndex >= 0 Then
            'Get the text of the selected item

            Dim selectedText As String = lstReceipt.SelectedItem.ToString()
            Dim parts() As String = selectedText.Split("-"c)
            Dim selectedID As Integer = CInt(parts(0).Trim())


            'Find the matching record by ID
            For i As Integer = 0 To RecordCount - 1
                If Customers(i).CustomerID = selectedID Then
                    txtCustomerID.Text = Customers(i).CustomerID.ToString()
                    txtName.Text = Customers(i).Name
                    txtEmail.Text = Customers(i).Email
                    txtPhone.Text = Customers(i).Phone
                    txtAddress.Text = Customers(i).Address
                    Exit For
                End If
            Next

        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            'Check for empty fields
            If txtName.Text.Trim() = "" Or txtEmail.Text.Trim() = "" Then
                MessageBox.Show("Name and Email cannot be empty")
                Exit Sub
            End If

            'Use of validation functions
            If Not IsValidEmail(txtEmail.Text) Then
                MessageBox.Show("Enter a valid email with @ and .")
                Exit Sub
            End If

            If Not IsValidID(txtCustomerID.Text) Then
                MessageBox.Show("Customer ID must be a positive number")
                Exit Sub
            End If

            'Get the Id from the textbox
            Dim idToUpdate As Integer = CInt(txtCustomerID.Text)

            'Search for the matching record in the array
            For i As Integer = 0 To RecordCount - 1
                If Customers(i).CustomerID = idToUpdate Then
                    'Update the records with new valuesform the textboxes
                    Customers(i).Name = txtName.Text
                    Customers(i).Email = txtEmail.Text
                    Customers(i).Phone = txtPhone.Text
                    Customers(i).Address = txtAddress.Text

                    'Confirmation message
                    MessageBox.Show("Records updated succesfully.")

                    'Refresh the listbox so the updated name shows
                    btnBrowse.PerformClick()
                    Exit Sub
                End If
            Next

            'if no matchoing ID was found
            MessageBox.Show("Customer ID not found.")
        Catch ex As Exception
            MessageBox.Show("Error updating record: " & ex.Message)
        End Try

        'User-defined validation functions


    End Sub

    'User-defined Boolean Function to validate email format
    Function IsValidEmail(Email As String) As Boolean
        'Simple check - must have @ and .
        If Email.Contains("@") And Email.Contains(".") Then
            Return True
        Else
            Return False
        End If
    End Function

    Function IsValidID(idText As String) As Boolean
        Dim id As Integer
        Return Integer.TryParse(idText, id) AndAlso id > 0
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            'Open the file for writing (overwriting exixsting content)
            Dim sw As New StreamWriter("customers.txt")

            'Write each record back to the file
            For i As Integer = 0 To RecordCount - 1
                sw.WriteLine($"{Customers(i).CustomerID}, {Customers(i).Name}, {Customers(i).Email}, {Customers(i).Phone}, {Customers(i).Address}")
            Next

            sw.Close()

            'Confirmation message
            MessageBox.Show("Data has been saved succesfully and is ready for next use.")
        Catch ex As Exception

            MessageBox.Show("Error saving file: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim searchTerm As String = txtSearch.Text.Trim().ToLower()
        lstReceipt.Items.Clear()

        For i As Integer = 0 To RecordCount - 1
            If Customers(i).Name.ToLower().Contains(searchTerm) Then
                lstReceipt.Items.Add($"{Customers(i).CustomerID}, {Customers(i).Name}, {Customers(i).Email}, {Customers(i).Phone}, {Customers(i).Address}")
            End If
        Next

        If lstReceipt.Items.Count = 0 Then
            MessageBox.Show("No matching records found.")
        End If
    End Sub

    Private Sub btnSort_Click(sender As Object, e As EventArgs) Handles btnSort.Click
        'Simple bubble sort by Name

        For i As Integer = 0 To RecordCount - 2
            For j As Integer = i + 1 To RecordCount - 1
                If String.Compare(Customers(i).Name, Customers(j).Name) > 0 Then
                    'Swap Records

                    Dim temp As Customer = Customers(i)
                    Customers(i) = Customers(j)
                    Customers(j) = temp
                End If
            Next
        Next

        'Refresh the listbox
        btnBrowse.PerformClick()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Ask user for confirmation

        Dim response As DialogResult
        response = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        'if they click yes, close the application
        If response = DialogResult.Yes Then
            Application.Exit()
        End If

    End Sub
End Class
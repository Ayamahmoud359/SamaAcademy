function GetDepartmentCategories(departmentId) {
    console.log("GetDepartmentCategories");
    console.log(departmentId);
    $.ajax({
        type: "POST",
        url: "/admin/Trainers/AddTrainer?handler=CategoriesDepartment",

        data: { departmentId: departmentId }, // Include both value and userId in the data object,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },

        success: function (response) {
            console.log("fefefe")
            console.log(response)
            //selectDiv.style.display = "block"; // Set the display style property to "block";
            $("#CategoryDepartmentId").html(response);


        },
        failure: function (response) {
            alert(response);
        }
    });

}


function validateForm() {

    var CatId = document.getElementById("CategoryId").value
    console.log(CatId)
    var CatSpanId = document.getElementById("CategorySpanId")

    if (CatId == 0) {
        CatSpanId.style.display = "flex";
        return false;
    }


    return true;
}
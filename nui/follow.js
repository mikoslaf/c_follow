document.getElementById("submitButton").addEventListener("click", function(event) {
    event.preventDefault();
  
    var data1 = document.getElementById("data1").value;
    var data2 = document.getElementById("data2").value;
    var data3 = document.getElementById("data3").value;
  
    console.log("Dane 1:", data1);
    console.log("Dane 2:", data2);
    console.log("Dane 3:", data3);
  });
  
window.addEventListener("message", function (event) {   
  if(event.data.action == "start") {
    this.document.body.style.display = "block";
  }
});

window.addEventListener("message", function (event) {   
  if(event.data.action == "start") {
    this.document.body.style.display = "block";
  }
});

document.getElementById("submitButton").addEventListener("click", function(event) {
  event.preventDefault();

  let Model = document.getElementById("data1").value; 
  let Cont = parseInt(document.getElementById("data2").value);
  if(isNaN(Cont)) {
    Cont = 4; 
  }
  let armed = document.getElementById("check1").checked;
  let combat = document.getElementById("check2").checked;
  let weapon = document.getElementById("data3").value;

  this.document.body.style.display = "none";
  $.post("https://c_follow/spawn", JSON.stringify({
      Model: Model,
      Cont: Cont,
      armed: armed,
      combat: combat,
      weapon: weapon,
  }));
});
$(document).ready(function() {
	let game_id = null;
	let req = null;
	
	$("#c_1_1").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-0-0", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_1_2").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-0-1", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_1_3").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-0-2", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_2_1").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-1-0", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_2_2").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-1-1", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_2_3").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-1-2", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_3_1").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-2-0", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_3_2").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-2-1", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	$("#c_3_3").click(function() {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?set-2-2", true);
		req.onreadystatechange = reqReadyStateChange;
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	
	$("#play").click(function() {
		game_id = new Date().getTime();
		req = new XMLHttpRequest();
		req.open("GET", "server_command?start", true);
		req.onreadystatechange = reqReadyStateChange;
		
		req.setRequestHeader("Name", game_id);
		req.send();
	});
	
	function reqReadyStateChange() {
			let status = req.status;
			if (status === 200) {
				let answ = req.responseText;
				answ = answ.split("-");
				let ans = answ[0];
				$("#answer").html(ans);
				console.log("0: " + ans[0]);
				switch (ans[0]){
					case ("0"): {$("#c_1_1").html("_"); break;};
					case ("1"): {$("#c_1_1").html("x"); break;};
					case ("2"): {$("#c_1_1").html("o"); break;};
				};
				console.log("1: " + ans[0]);
				switch (ans[1]){
					case ("0"): {$("#c_1_2").html("_"); break;};
					case ("1"): {$("#c_1_2").html("x"); break;};
					case ("2"): {$("#c_1_2").html("o"); break;};
				}
				console.log("2: " + ans[0]);
				switch (ans[2]){
					case ("0"): {$("#c_1_3").html("_"); break;};
					case ("1"): {$("#c_1_3").html("x"); break;};
					case ("2"): {$("#c_1_3").html("o"); break;};
				}
				console.log("3: " + ans[0]);
				switch (ans[3]){
					case ("0"): {$("#c_2_1").html("_"); break;};
					case ("1"): {$("#c_2_1").html("x"); break;};
					case ("2"): {$("#c_2_1").html("o"); break;};
				}
				console.log("4: " + ans[0]);
				switch (ans[4]){
					case ("0"): {$("#c_2_2").html("_"); break;};
					case ("1"): {$("#c_2_2").html("x"); break;};
					case ("2"): {$("#c_2_2").html("o"); break;};
				}
				console.log("5: " + ans[0]);
				switch (ans[5]){
					case ("0"): {$("#c_2_3").html("_"); break;};
					case ("1"): {$("#c_2_3").html("x"); break;};
					case ("2"): {$("#c_2_3").html("o"); break;};
				}
				console.log("6: " + ans[0]);
				switch (ans[6]){
					case ("0"): {$("#c_3_1").html("_"); break;};
					case ("1"): {$("#c_3_1").html("x"); break;};
					case ("2"): {$("#c_3_1").html("o"); break;};
				}
				console.log("7: " + ans[0]);
				switch (ans[7]){
					case ("0"): {$("#c_3_2").html("_"); break;};
					case ("1"): {$("#c_3_2").html("x"); break;};
					case ("2"): {$("#c_3_2").html("o"); break;};
				}
				console.log("8: " + ans[0]);
				switch (ans[8]){
					case ("0"): {$("#c_3_3").html("_"); break;};
					case ("1"): {$("#c_3_3").html("x"); break;};
					case ("2"): {$("#c_3_3").html("o"); break;};
				}
				console.log(answ[1]);
				switch (answ[1]){
					case ("w"): {$("#answer").html("win"); break;}
					case ("l"): {$("#answer").html("loose"); break;}
					case ("d"): {$("#answer").html("draw"); break;}
				}
			}
		}
		
	function setToken(cell_id) {
		req = new XMLHttpRequest();
		req.open("GET", "server_command?"+cell_id, false);
		
		req.onreadystatechange = reqReadyStateChange;
		
		req.setRequestHeader("Name", game_id);
		req.send();
	}
	
	
})
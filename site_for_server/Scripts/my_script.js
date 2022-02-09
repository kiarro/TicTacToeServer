$(document).ready(function() {
	"use strict";
	console.log("my_script.is loaded");
	
	paper.install(window);
	let canvas = document.getElementById("mainCanvas"); 
	paper.setup(canvas);
	
	// var tool = new Tool();
	// tool.onMouseDown = function(event) {
		// var c = Shape.Circle(event.point, 80);
		// c.fillColor = "green";
		// var txt = new PointText(event.point);
		// txt.justification = "center";
		// txt.fillColor = "white";
		// txt.fontSize = 20;
		// txt.content = "hello world";
		// paper.view.draw();
	// }
		
	let btn = document.getElementById("play");
	let res = document.getElementById("result");
	btn.onclick = function() {
		console.log("game started");
		let funds = 50;
		while (funds>0 && funds<100){
			const bets = {crown: 0, anchor: 0, heart: 0, 
				spade: 0, club: 0, diamond: 0};
			let totalBet = rand(1, funds);
			if (totalBet === 7) {
				totalBet = funds;
				bets.heart = totalBet;
			} else {
				let remaining = totalBet;
				do {
					let bet = rand(1, remaining);
					let face = randFace();
					bets[face] = bets[face] + bet;
					remaining = remaining - bet;
				} while(remaining > 0);
			}
			funds = funds - totalBet;
			const hand = [];
			for (let roll = 0; roll < 3; roll++) {
				hand.push(randFace());
			}
			let winnings = 0;
			for (let die=0; die < hand.length; die++) {
				const face = hand[die];
				if (bets[face] > 0) {
					winnings = winnings + bets[face]; 
					console.log(face + ": " + bets[face]);
				}
			}
			
		}
		console.log("game ends with fund: " + funds);
	}
	
	
	function rand(m, n) {
		return m+Math.floor((n-m+1)*Math.random());
	}
	
	function randFace() {
		return ["crown", "anchor", "heart", "spade", "club", "diamond"][rand(0, 5)];
	}
	
	
	
});
const WebSocket = require('ws')
const wss = new WebSocket.Server({port: 8080}, ()=>{
	console.log('server.started')
})

wss.on('connection',(ws)=>{
	ws.on('message',(data)=>{
		console.log('data recieved: ', data.toString())
		ws.send(data.toString())
	})
})

wss.on('listening',()=>{
	console.log('server is listening to port 8080')
})
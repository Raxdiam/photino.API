var F=Object.defineProperty;var R=(t,i,d)=>i in t?F(t,i,{enumerable:!0,configurable:!0,writable:!0,value:d}):t[i]=d;var h=(t,i,d)=>(R(t,typeof i!="symbol"?i+"":i,d),d);(function(t,i){typeof exports=="object"&&typeof module!="undefined"?i(exports):typeof define=="function"&&define.amd?define(["exports"],i):(t=typeof globalThis!="undefined"?globalThis:t||self,i(t.PhotinoAPI={}))})(this,function(t){"use strict";function i(){let n=new Date().getTime();return"xxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g,s=>{const r=(n+Math.random()*16)%16|0;return n=Math.floor(n/16),(s=="x"?r:r&3|8).toString(16)})}class d{constructor(e){h(this,"id");h(this,"data");this.id=i(),this.data=e}}class o{send(e,...s){return t.Photino.send(this.name,e,...s)}}class c extends o{constructor(){super(...arguments);h(this,"name","app")}exit(e){return this.send(this.exit.name,e)}cwd(){return this.send(this.cwd.name)}}class f extends o{constructor(){super(...arguments);h(this,"name","io")}readFile(e){return this.send(this.readFile.name,e)}readFileText(e,s=null){return this.send(this.readFileText.name,e,s)}readFileLines(e,s=null){return this.send(this.readFileLines.name,e,s)}writeFile(e,s){return this.send(this.writeFile.name,e,s)}writeFileText(e,s,r=null){return this.send(this.writeFileText.name,e,s,r)}writeFileLines(e,s,r=null){return this.send(this.writeFileLines.name,e,s,r)}listFiles(e,s=null,r=!1){return this.send(this.listFiles.name,e,s,r)}listFolders(e,s=null,r=!1){return this.send(this.listFolders.name,e,s,r)}createFolder(e){return this.send(this.createFolder.name,e)}deleteFile(e){return this.send(this.deleteFile.name,e)}deleteFolder(e,s=!1){return this.send(this.deleteFolder.name,e,s)}fileExists(e){return this.send(this.fileExists.name,e)}folderExists(e){return this.send(this.folderExists.name,e)}resolvePath(e){return this.send(this.resolvePath.name,e)}getExtension(e){return this.send(this.getExtension.name,e)}}class M extends o{constructor(){super(...arguments);h(this,"name","osModule")}isWindows(){return this.send(this.isWindows.name)}isLinux(){return this.send(this.isLinux.name)}isMacOs(){return this.send(this.isMacOs.name)}getEnvar(e){return this.send(this.getEnvar.name,e)}cmd(e){return this.send(this.cmd.name,e)}}class T extends o{constructor(){super(...arguments);h(this,"name","window")}getTitle(){return this.send(this.getTitle.name)}getMaximized(){return this.send(this.getMaximized.name)}getMinimized(){return this.send(this.getMinimized.name)}getDevToolsEnabled(){return this.send(this.getDevToolsEnabled.name)}getContextMenuEnabled(){return this.send(this.getContextMenuEnabled.name)}getTopMost(){return this.send(this.getTopMost.name)}getCentered(){return this.send(this.getCentered.name)}getChromeless(){return this.send(this.getChromeless.name)}getFullScreen(){return this.send(this.getFullScreen.name)}getResizable(){return this.send(this.getResizable.name)}getWidth(){return this.send(this.getWidth.name)}getHeight(){return this.send(this.getHeight.name)}getTop(){return this.send(this.getTop.name)}getLeft(){return this.send(this.getLeft.name)}setTitle(e){return this.send(this.setTitle.name,e)}setMaximized(e){return this.send(this.setMaximized.name,e)}setMinimized(e){return this.send(this.setMinimized.name,e)}setDevToolsEnabled(e){return this.send(this.setDevToolsEnabled.name,e)}setContextMenuEnabled(e){return this.send(this.setContextMenuEnabled.name,e)}setTopMost(e){return this.send(this.setTopMost.name,e)}setChromeless(e){return this.send(this.setChromeless.name,e)}setFullScreen(e){return this.send(this.setFullScreen.name,e)}setResizable(e){return this.send(this.setResizable.name,e)}setWidth(e){return this.send(this.setWidth.name,e)}setHeight(e){return this.send(this.setHeight.name,e)}setTop(e){return this.send(this.setTop.name,e)}setLeft(e){return this.send(this.setLeft.name,e)}close(){return this.send(this.close.name)}load(e){return this.send(this.load.name,e)}loadRawString(e){return this.send(this.loadRawString.name,e)}center(){return this.send(this.center.name)}hitTest(e){t.Photino.sendRaw(`ht:${e}`)}drag(){t.Photino.sendRaw("ht:drag")}resizeTopLeft(){t.Photino.sendRaw("ht:nw")}resizeTop(){t.Photino.sendRaw("ht:n")}resizeTopRight(){t.Photino.sendRaw("ht:ne")}resizeRight(){t.Photino.sendRaw("ht:e")}resizeBottomRight(){t.Photino.sendRaw("ht:se")}resizeBottom(){t.Photino.sendRaw("ht:s")}resizeBottomLeft(){t.Photino.sendRaw("ht:sw")}resizeLeft(){t.Photino.sendRaw("ht:w")}}const m=window;t.Photino=void 0,function(n){n.app=new c,n.io=new f,n.os=new M,n.window=new T;async function e(l,u,...g){const a=await s({module:l,method:u,parameters:g});if(a.error)throw new Error(a.error);return a.result}n.send=e;function s(l){var u=new d(l);return m.external.sendMessage(`api:${JSON.stringify(u)}`),new Promise(g=>{const w=setTimeout(()=>{throw new Error("Request timed out")},1e4);m.external.receiveMessage(a=>{if(!a.startsWith("api:"))return;const x=JSON.parse(a.substring(4));x.id===u.id&&(clearTimeout(w),g(x.data))})})}async function r(l){m.external.sendMessage(l)}n.sendRaw=r}(t.Photino||(t.Photino={})),t.PhotinoModule=o,Object.defineProperty(t,"__esModule",{value:!0}),t[Symbol.toStringTag]="Module"});
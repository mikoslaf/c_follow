fx_version 'bodacious'
game 'gta5'

ui_page 'nui/index.html'

files {
    'Client/bin/Release/**/publish/*.dll',
    'nui/index.html',
    'nui/follow.js',
    'nui/style.css'
}

client_script 'Client/bin/Release/**/publish/*.net.dll'
server_script 'Server/bin/Release/**/publish/*.net.dll'

author 'mikoslaf'
version '1.0.0'
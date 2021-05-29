@echo off
if not defined TAG (
    set TAG=1
    start wt -p "cmd" -d %cd% %0
    exit
)

chcp 65001
yarn build
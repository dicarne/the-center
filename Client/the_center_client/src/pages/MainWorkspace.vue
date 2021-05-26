<template>
    <div class="main-work-area">
        <a-row>
            <a-col span="6">
                <a-input v-model:value="env.wName" @change="renameWorkspace" />
            </a-col>
            <a-col span="6">
                <a-button @click="getboard">刷新</a-button>
            </a-col>
            <a-col span="6">
                <a-button @click="openSortBoards">排序</a-button>
            </a-col>
            <a-col span="6">
                <a-button @click="deleteWorkspace">删除</a-button>
            </a-col>
        </a-row>
    </div>

    <Row :gutter="[16, 16]">
        <Col :span="6">
            <div class="card-board card-body">
                <a-button @click="createBoard" style="margin-top: 15%;">+</a-button>
            </div>
        </Col>
        <Col v-for="item in list.boards" :key="item.id" :span="item.w">
            <BoardCard
                :ver="item.ver"
                :u-i-coms="item.uIComs"
                :workspace="workspace"
                :boardid="item.id"
                :getboard="getboard"
                :board="item"
                :environment="list"
            />
        </Col>
    </Row>
    <a-modal title="增加新卡片" v-model:visible="newcard_visiable" @ok="newcard_ok">
        <a-menu v-model:selectedKeys="createCard.select" @click="createCard.click">
            <a-menu-item :key="item.type" v-for="item in createCard.cardTypes">
                <p>{{ item.name }}</p>
            </a-menu-item>
        </a-menu>
    </a-modal>
    <a-modal title="排序卡片" v-model:visible="b_openSortBoards" @ok="sortBoards">
        <draggable
            v-model="borderOrder"
            @start="sort_drag = true"
            @end="sort_drag = false"
            item-key="id"
        >
            <template #item="{ element }">
                <div class="sort-item">{{ element.name }}</div>
            </template>
        </draggable>
    </a-modal>
</template>

<script lang="ts">
import { createVNode, defineComponent, onMounted, onUnmounted, PropType, provide, reactive, ref } from "vue";
import { Row, Col, Modal } from "ant-design-vue";
import { BoardUI, CreateBoard, DeleteWorkspace, FocusWorkspace, GetAllBoardTypes, GetBoards, ModuleTypeNamePair, onConnected, RenameWorkspace, SortBoards, WorkspaceDesc } from "../api/workspace";
import BoardElement from "../components/BoardElement.vue"
import BoardCard from "./BoardCard.vue"
import { workspaces } from "../connection/Server"
import draggable from 'vuedraggable'
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";

export default defineComponent({
    components: {
        Row,
        Col,
        BoardElement,
        BoardCard,
        draggable
    },
    props: {
        workspace: {
            type: String,
            validator(this: void, v: string) {
                return !!v;
            },
            required: true,
        },
        workspaceObj: {
            type: Object as PropType<WorkspaceDesc>,
            required: true
        },
        reload: {
            type: Function,
            required: true
        }
    },
    setup: (prop) => {
        const list = reactive({boards: [] as BoardUI[]});

        const workspaceObj = ref(prop.workspaceObj)
        provide("workspace", workspaceObj)

        const getboard = async () => {
            list.boards = (await GetBoards(prop.workspace)).filter(b => !b.hide);
            list.boards.forEach(u => { if (u.ver === undefined) u.ver = 0 })
        };

        onConnected(async () => {
            try {
                await FocusWorkspace(prop.workspace)
                await getboard()
            } catch (error) {
                console.error(error)
            }

        });

        const dispatchBoard = (board: string, data: string) => {
            let jdata = JSON.parse(data)
            let index = list.boards.findIndex(u => u.id === board)
            list.boards[index].uIComs = ref(jdata)
            list.boards[index].ver++
        }
        onMounted(() => {
            if (prop.workspace != "home") {
                workspaces.set(prop.workspace, dispatchBoard)
            }
        })
        onUnmounted(() => {
            workspaces.delete(prop.workspace)
        })

        // 创建卡片
        const createCard = reactive({
            cardTypes: [] as ModuleTypeNamePair[],
            select: [] as string[],
            click: (e: any) => {

            }
        })
        const newcard_visiable = ref(false)
        const newcard_ok = async () => {
            if (createCard.select.length != 0) {
                await CreateBoard(prop.workspace, createCard.select[0])
                await getboard()
                newcard_visiable.value = false
            }

        }
        const createBoard = async () => {
            newcard_visiable.value = true
            createCard.cardTypes = await GetAllBoardTypes();
        }
        // ------
        // 排序卡片
        const borderOrder = ref<{ id: string, name: string }[]>([])
        const b_openSortBoards = ref(false)
        const openSortBoards = () => {
            b_openSortBoards.value = true;
            borderOrder.value = list.boards.map(l => { return { id: l.id, name: l.cName } })
        }
        const sortBoards = async () => {
            await SortBoards(prop.workspace, borderOrder.value.map(l => l.id))
            b_openSortBoards.value = false
            await getboard()
        }
        const sort_drag = ref(false)
        // ------
        // 重命名空间
        const renameWorkspace = async (e: any) => {
            await RenameWorkspace(prop.workspace, prop.workspaceObj.wName)
        }
        // ------
        // 删除空间
        const deleteWorkspace = () => {
            Modal.confirm({
                title: '该操作不可撤销，请谨慎操作！',
                icon: createVNode(ExclamationCircleOutlined),
                content: '你确定要删除此工作空间？',
                okText: '删除',
                okType: 'danger',
                cancelText: '取消',
                async onOk() {
                    await DeleteWorkspace(prop.workspace)
                    prop.reload(prop.workspace)
                },
                onCancel() {
                },
            });

        }
        // ------
        return {
            getboard, list, createBoard, workspace: prop.workspace, newcard_visiable, newcard_ok, createCard, openSortBoards, sortBoards, sort_drag,
            b_openSortBoards, borderOrder, env: prop.workspaceObj, renameWorkspace, deleteWorkspace
        };
    },
});
</script>

<style>
.main-work-area {
    padding-bottom: 10px;
}
.card-body {
    background-color: #fff;
    padding: 28px 10px 18px 10px;
}
.card-board {
    border-radius: 8px;
    box-shadow: 0px 0px 10px #e6e6e6;
    position: relative;
    min-height: 180px;
}
.card-board:hover {
    box-shadow: 0px 0px 10px #d3d3d3;
}
.sort-item {
    border: 1px solid rgba(0, 0, 0, 0.125);
    background-color: #fff;
    padding: 8px;
}
</style>

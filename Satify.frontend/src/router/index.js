import Vue from "vue";
import VueRouter from "vue-router";
import Inventory from "@/views/Inventory.vue";
import Customers from "@/views/Customers.vue";
import Orders from "@/views/Orders.vue";
import CreateInvoice from "@/views/CreateInvoice.vue";
import Salespersons from "@/views/Salespersons.vue";
import CustomerDetails from "@/views/CustomerDetails.vue";
Vue.use(VueRouter);
const routes = [
    {
        path: '/',
        name: 'home',
        component: Inventory,
    },
    {
        path: '/inventory',
        name: 'inventory',
        component: Inventory,
    },
    {
        path: '/customers',
        name: 'customers',
        component: Customers,
    },
    {
        path: '/orders',
        name: 'order',
        component: Orders,
    },
    {
        path: '/invoice/new',
        name: 'create-invoice',
        component: CreateInvoice,
    },
    {
        path: '/salespersons',
        name: 'salespersons',
        component: Salespersons,
    },
    {
        path: '/customer-details/:customerId',
        name: 'customer-details',
        component: CustomerDetails,
    }
];
const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes,
});
export default router;
//# sourceMappingURL=index.js.map
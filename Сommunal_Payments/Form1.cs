using System;
using System.Linq;
using System.Windows.Forms;

namespace Сommunal_Payments
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int elecrtTotal, gasTotal, waterTotal;
        double elecrtMoney;
        

        #region Электроэнергия
        private void Electricity(object sender, EventArgs e)
        {
            try
            {
                if (elecrtBfrIndication != null && elecrtNowIndication != null && Convert.ToInt32(elecrtNowIndication.Text) > 0 && Convert.ToInt32(elecrtBfrIndication.Text) > 0)
                {
                    //общее количество потребленной электроэнергии
                    elecrtTotal = Convert.ToInt32(elecrtNowIndication.Text) - Convert.ToInt32(elecrtBfrIndication.Text);

                    if (elecrtTotal > 0)
                        elecrtTotalIndication.Text = elecrtTotal.ToString() + " кВт/ч";
                    else
                        elecrtTotalIndication.Text = "0";

                    //выводим денежную сумму к оплате за электроэнергию
                    //если более 100 кВт/ч
                    if (elecrtTotal > 100)
                    {
                        elecrtMoney = (elecrtTotal - 100) * Convert.ToDouble(elecrtAfr100Money.Text) + (100 * Convert.ToDouble(elecrtBfr100Money.Text));
                        if (elecrtMoney > 0)
                        {
                            elecrtTotalMoney.Text = elecrtMoney.ToString();
                        }
                        else
                        {
                            elecrtTotalMoney.Text = "0";
                        }
                    }
                    //если менее 100 кВт/ч
                    else if (elecrtTotal <= 100)
                    {
                        elecrtMoney = elecrtTotal * Convert.ToDouble(elecrtBfr100Money.Text);
                        if (elecrtMoney > 0)
                            elecrtTotalMoney.Text = elecrtMoney.ToString();
                        else
                            elecrtTotalMoney.Text = "0";
                    }

                }

            }

            catch (Exception)
            {
                elecrtTotalIndication.Text = "0";
                elecrtTotalMoney.Text = "0";
            }

        }

        private void checkBoxElectric_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxElectric.Checked == true)
            {
                electricTarif.Enabled = true;
            }
            else
            {
                electricTarif.Enabled = false;
            }
        }
        #endregion

        #region Отопление
        private void Heating(object sender, EventArgs e)
        {
            try
            {
                if (tbHeatingSquare.Text != "" && tbHeatingTarif.Text != "" && Convert.ToInt32(tbHeatingSquare.Text) > 0 && Convert.ToDouble(tbHeatingTarif.Text) > 0.0)
                {
                    HeatingMoney.Text = HeatingTotal.Text = (Convert.ToDouble(tbHeatingSquare.Text) * Convert.ToDouble(tbHeatingTarif.Text)).ToString();
                }
                else
                {
                    HeatingMoney.Text = HeatingTotal.Text = "0";
                }
            }
            catch (Exception)
            { }
        }
        #endregion

        #region Вода
        private void Water(object sender, EventArgs e)
        {
            try
            {
                if (waterBfrIndication.Text != "" && waterNowIndication.Text != "" && Convert.ToInt32(waterBfrIndication.Text) > 0 && Convert.ToInt32(waterNowIndication.Text) > 0)
                {
                    //выводим общее количество потребленной воды 
                    waterTotal = Convert.ToInt32(waterNowIndication.Text) - Convert.ToInt32(waterBfrIndication.Text);

                    if (waterTotal > 0)
                    {
                        waterTotalIndication.Text = waterTotal.ToString() + " куб. м";
                        //выводим денежную сумму к оплате за воду
                        waterTotalMoney.Text = (Convert.ToDouble(waterTotal) * Convert.ToDouble(tbWaterTarif.Text)).ToString();
                    }
                    else
                    {
                        waterTotalIndication.Text = "0";
                        waterTotalMoney.Text = "0";
                    }
                }
                else
                {
                    waterTotalIndication.Text = "0";
                    waterTotalMoney.Text = "0";
                }
            }

            catch (Exception)
            {
                waterTotalIndication.Text = "0";
                waterTotalMoney.Text = "0";
            }
        }

        //Чек-бокс вода тариф
        private void checkBoxWater_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWater.Checked)
            {
                waterTarifLbl.Enabled = true;
                tbWaterTarif.Enabled = true;
            }
            else
            {
                waterTarifLbl.Enabled = false;
                tbWaterTarif.Enabled = false;
            }

        }
        #endregion

        #region Проверка ввода цифр
        private void DigitOnly(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8 && e.KeyChar != 44) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Газ 
        private void Gas(object sender, EventArgs e)
        {

            //определяем на основании данных с какого поля работаем
            switch (GBGasIndication.Enabled)
            {
                //если счетчик ЕСТЬ 
                #region Case True
                case true:

                    if (tbGasBfrIndication.Text != "" && tbGasNowIndication.Text != "" && Convert.ToInt32(tbGasBfrIndication.Text) > 0 && Convert.ToInt32(tbGasNowIndication.Text) > 0)
                    {
                        //выводим общее количество потребленной электроэнергии
                        gasTotal = Convert.ToInt32(tbGasNowIndication.Text) - Convert.ToInt32(tbGasBfrIndication.Text);

                        if (gasTotal > 0)
                        {
                            gasTotalIndication.Text = gasTotal.ToString() + " куб/м";
                            gasTotalMoney.Text = (Convert.ToDouble(gasTotal) * 6.8).ToString();
                        }
                        else
                        {
                            gasTotalIndication.Text = "0";
                            gasTotalMoney.Text = "0";
                        }
                    }
                    else
                    {
                        gasTotalIndication.Text = "0";
                        gasTotalMoney.Text = "0";
                    }
                    break;
                #endregion

                //если счетчика НЕТ
                #region Case False
                case false: 

                    if (GasGroup.Text != "" && GasPeopleCount.Text != "")

                    {
                        switch (GasGroup.Text.ElementAt(0))
                        {
                            case '1':
                                tbGasTarif.Text = "28,22";
                                GasNorms.Text = "3.3 куб/м";
                                gasNoCounterTotalMoney.Text = (Convert.ToDouble(tbGasTarif.Text) * Convert.ToDouble(GasPeopleCount.Text)).ToString();
                                break;

                            case '2':
                                tbGasTarif.Text = "46,17";
                                GasNorms.Text = "5.4 куб/м";
                                gasNoCounterTotalMoney.Text = (Convert.ToDouble(tbGasTarif.Text) * Convert.ToDouble(GasPeopleCount.Text)).ToString();
                                break;

                            case '3':
                                tbGasTarif.Text = "89,78";
                                GasNorms.Text = "10.5 куб/м";
                                gasNoCounterTotalMoney.Text = (Convert.ToDouble(tbGasTarif.Text) * Convert.ToDouble(GasPeopleCount.Text)).ToString();
                                break;
                        }
                    }
                    break;
                    #endregion
            }
        }

        //Чек-бокс газ Нет счетчика
        private void checkBoxGas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxGas.Checked == true)
            {
                GBGasNoСounter.Enabled = true;
                GBGasIndication.Enabled = false;
                gasTotalIndication.Text = gasTotalMoney.Text = "0";
            }
            else
            {
                GBGasNoСounter.Enabled = false;
                GBGasIndication.Enabled = true;
                GasNorms.Text = gasNoCounterTotalMoney.Text = "0";
            }
        }
        #endregion

        //Считаем общую денежную сумму
        private void TotalBill(object sender, EventArgs e)
        {
            totalBillLabel.Text = (Convert.ToDouble(elecrtTotalMoney.Text) +
                Convert.ToDouble(gasTotalMoney.Text) +
                Convert.ToDouble(gasNoCounterTotalMoney.Text) +
                Convert.ToDouble(HeatingMoney.Text) +
                Convert.ToDouble(waterTotalMoney.Text)).ToString() + "грн.";
        }



    }
}

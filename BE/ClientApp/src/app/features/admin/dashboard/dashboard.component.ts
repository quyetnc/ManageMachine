import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/core/services/api.service';
import { ChartConfiguration, ChartData, ChartEvent, ChartType } from 'chart.js';

interface DashboardStats {
  totalMachines: number;
  totalTypes: number;
  machinesByType: { typeName: string, count: number }[];
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  loading = false;

  // Pie Chart Configuration
  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'top',
      }
    }
  };
  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: [],
    datasets: [{ data: [] }]
  };
  public pieChartType: ChartType = 'pie';

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    this.loading = true;
    this.api.get('Dashboard/stats').subscribe({
      next: (res: any) => {
        this.stats = res;
        this.updateCharts();
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load dashboard stats', err);
        this.loading = false;
      }
    });
  }

  updateCharts() {
    if (!this.stats) return;

    this.pieChartData = {
      labels: this.stats.machinesByType.map(x => x.typeName),
      datasets: [
        {
          data: this.stats.machinesByType.map(x => x.count),
          backgroundColor: [
            '#6366F1', '#EC4899', '#10B981', '#F59E0B', '#3B82F6', '#8B5CF6'
          ]
        }
      ]
    };
  }
}
